using Core.Querying.Find.Models.Response;
using Core.Querying.Find.SearchRequests;
using EPiServer.Commerce.Catalog.ContentTypes;

namespace Core.Querying.Find.Interfaces
{
    public interface ISearchService
    {
        SearchResponse<TEntry> SearchProduct<TEntry>(IProductSearchRequest request) where TEntry : ProductContent;
    }
}