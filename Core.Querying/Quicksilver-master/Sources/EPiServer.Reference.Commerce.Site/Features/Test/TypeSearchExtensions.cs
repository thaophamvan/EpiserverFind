using System.Threading;
using System.Threading.Tasks;
using EPiServer.Find;

namespace EPiServer.Reference.Commerce.Site.Features.Test
{
    internal static class TypeSearchExtensions
    {
        public static async Task<SearchResults<TResult>> GetResultAsync<TResult>(this ITypeSearch<TResult> search, ITicketProvider ticketProvider)
        {
            await ticketProvider.WaitAsync(CancellationToken.None);

            return search.GetResult();
        }
    }
}