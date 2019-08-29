using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Querying.Infrastructure.ProtectedCall
{
    public interface IAsyncCircuitBreaker
    {
        Task<T> ExecuteAsync<T>(Func<T> operation, Func<T> openStateOperation);
    }
}
