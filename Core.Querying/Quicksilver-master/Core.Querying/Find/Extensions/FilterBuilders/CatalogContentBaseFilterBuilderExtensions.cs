using Core.Querying.Find.Models.Request;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Find;

namespace Core.Querying.Find.Extensions.FilterBuilders
{
    public static class CatalogContentBaseFilterBuilderExtensions
    {
        public static ITypeSearch<TEntry> FilterByLanguage<TEntry>(this ITypeSearch<TEntry> typeSearch, IFilterRequest request) where TEntry : CatalogContentBase
        {
            if (request?.Filters?.Language == null)
            {
                return typeSearch;
            }

            var suffix = request.Filters?.Language.FieldSuffix;
            return TypeSearchExtensions.Filter<TEntry>(typeSearch, (TEntry p) => EPiServer.Find.Filters.Match(p.Language.Name, suffix));
        }
    }
}