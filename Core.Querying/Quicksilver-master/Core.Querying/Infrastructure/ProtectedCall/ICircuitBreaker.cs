using System;

namespace Core.Querying.Infrastructure.ProtectedCall
{
    /// <summary>
    /// Using PolicyWrap including two Fallback policies (for different exceptions), WaitAndRetry and CircuitBreaker.
    /// Keeping track of each requested item and reporting server failures when encoounting exception
    /// Usesing Fallback policies to fallback the secondary services or provide default values, when the call still fails overall.
    /// 
    /// Discussion:  What if the underlying system was limit request per second or completely down?  
    /// Keeping retrying would be pointless...
    /// ... and would leave the client hanging, retrying for successes which never come.
    /// 
    /// Enter circuit-breaker: 
    /// After too many failures, breaks the circuit for a period, during which it blocks calls + fails fast.
    /// - protects the downstream system from too many calls if it's really struggling (reduces load, so it can recover)
    /// - allows the client to get a fail response _fast, not wait for ages, if downstream is down.
    /// - use the same kind of policy(Fallback services)
   
    /// </summary>
    public interface ICircuitBreaker
    {
        T Execute<T>(Func<T> operation, Func<T> openStateOperation);
    }
}
