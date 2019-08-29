using Core.Querying.Find.Models.Request;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Find;

namespace Core.Querying.Services
{
    public interface ISearchServices
    {
        ITypeSearch<TEntry> GeneralSearch<TEntry>(ISearchRequest request) where TEntry : CatalogContentBase;
    }
}
