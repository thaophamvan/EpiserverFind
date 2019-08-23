using System;
using System.Web;
using Polly;
using Polly.CircuitBreaker;

namespace Core.Querying.ConcurrentQueue
{
    public interface ICircuitBreakerPolicyProvider
    {
        CircuitBreakerPolicy MakeBreakerPolicy();

        AsyncCircuitBreakerPolicy MakeBreakerPolicyAsync();
    }

    public class CircuitBreakerPolicyProvider : ICircuitBreakerPolicyProvider
    {
        private readonly int _exceptionsAllowedBeforeBreaking;
        private readonly TimeSpan _durationOfBreak;

        public CircuitBreakerPolicyProvider(int exceptionsAllowedBeforeBreaking, TimeSpan durationOfBreak)
        {
            _exceptionsAllowedBeforeBreaking = exceptionsAllowedBeforeBreaking;
            _durationOfBreak = durationOfBreak;
        }

        public CircuitBreakerPolicy MakeBreakerPolicy()
        {
            return Policy.Handle<HttpException>(e => e.ErrorCode == 429).CircuitBreaker(_exceptionsAllowedBeforeBreaking, _durationOfBreak);
        }

        public AsyncCircuitBreakerPolicy MakeBreakerPolicyAsync()
        {
            return Policy.Handle<HttpException>(e => e.ErrorCode == 429).CircuitBreakerAsync(_exceptionsAllowedBeforeBreaking, _durationOfBreak);
        }
    }
}