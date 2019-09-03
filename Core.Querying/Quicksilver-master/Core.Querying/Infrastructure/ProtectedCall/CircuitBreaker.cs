using System;
using Core.Querying.Infrastructure.Configuration;
using EPiServer.Logging;
using Polly;
using Polly.CircuitBreaker;
using Polly.Fallback;
using Polly.Retry;
using Polly.Wrap;

namespace Core.Querying.Infrastructure.ProtectedCall
{
    public class CircuitBreaker : ICircuitBreaker
    {
        private readonly ILogger _log = LogManager.GetLogger(typeof(CircuitBreaker));
        public T Execute<T>(Func<T> operation, Func<T> openStateOperation)
        {
            // Define our waitAndRetry policy: keep retrying with n ms gaps.
            RetryPolicy waitAndRetryPolicy = Policy
                .Handle<Exception>(e => !(e is BrokenCircuitException)) // Exception filtering!  We don't retry if the inner circuit-breaker judges the underlying system is out of commission!
                .WaitAndRetryForever(
                    attempt => TimeSpan.FromMilliseconds(SiteSettings.Instance.SleepDurationsMilliseconds),
                    (exception, calculatedWaitDuration) =>
                    {
                        _log.Error(".Log, then retry: ", exception);
                    });

            // Define our CircuitBreaker policy: Break if the action fails n times in a row.
            CircuitBreakerPolicy circuitBreakerPolicy = Policy
                .Handle<Exception>()
                .CircuitBreaker(
                    exceptionsAllowedBeforeBreaking: SiteSettings.Instance.ExceptionsAllowedBeforeBreaking,
                    durationOfBreak: TimeSpan.FromSeconds(1),
                    onBreak: (ex, breakDelay) =>
                    {
                        _log.Error($".Breaker logging: Breaking the circuit for {breakDelay.TotalMilliseconds} + ms!");
                        _log.Error($"..due to: {ex.Message}");
                    },
                    onReset: () =>
                    {
                        _log.Error(".Breaker logging: Call ok! Closed the circuit again!");
                    },
                    onHalfOpen: () =>
                    {
                        _log.Error(".Breaker logging: Half-open: Next call is a trial!");
                    }
                );

            // Define a fallback policy: provide a secondary services or substitute result to the user, for any exception.
            FallbackPolicy<T> fallbackForAnyException = Policy<T>
                .Handle<Exception>()
                .Fallback(
                    fallbackAction: openStateOperation,
                    onFallback: e =>
                    {
                        _log.Error($"Fallback catches eventually failed with: {e.Exception.Message}");
                    }
                );

            // We combine the waitAndRetryPolicy and circuitBreakerPolicy into a PolicyWrap, using the *static* Policy.Wrap syntax.
            PolicyWrap myResilienceStrategy = Policy.Wrap(waitAndRetryPolicy, circuitBreakerPolicy);

            // We wrap the two fallback policies onto the front of the existing wrap too. And the fact that the PolicyWrap myResilienceStrategy from above is just another Policy, which can be onward-wrapped too.  
            // With this pattern, you can build an overall resilience strategy programmatically, reusing some common parts (eg PolicyWrap myResilienceStrategy) but varying other parts (eg Fallback) individually for different calls.
            PolicyWrap<T> policyWrap = fallbackForAnyException.Wrap(myResilienceStrategy);

            try
            {
                // Manage the call according to the whole policy wrap.
                var response =
                    policyWrap.Execute(operation);
                return response;

            }
            catch (Exception e) // try-catch not needed, now that we have a Fallback.Handle<Exception>.  It's only been left in to *demonstrate* it should never get hit.
            {
                _log.Error("Should never arrive here.  Use of fallbackForAnyException should have provided nice fallback value for any exceptions.", e);
                    
                return default(T);
            }
        }
    }
}
