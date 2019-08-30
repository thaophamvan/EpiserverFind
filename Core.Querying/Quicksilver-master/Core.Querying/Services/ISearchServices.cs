using Core.Querying.Find.Models.Request;
using Core.Querying.Find.Models.Response;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.Find;

namespace Core.Querying.Services
{
    public interface ISearchServices
    {
        SearchResponse<TEntry> GenericSearch<TEntry>(ISearchRequest request) where TEntry : IContent;
    }
}
