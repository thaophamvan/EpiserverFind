using System;
using System.Threading.Tasks;

namespace Core.Querying.Infrastructure.ProtectedCall
{
    public class AsyncCircuitBreaker : IAsyncCircuitBreaker
    {
        public Task<T> ExecuteAsync<T>(Func<T> operation, Func<T> openStateOperation)
        {
            throw new NotImplementedException();
        }
    }
}
