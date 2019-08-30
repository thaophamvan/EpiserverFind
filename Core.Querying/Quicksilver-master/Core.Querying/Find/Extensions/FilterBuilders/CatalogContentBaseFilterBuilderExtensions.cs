using System.Collections.Generic;
using Core.Querying.Find.Models.Request;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.Find;
using EPiServer.Find.Cms;

namespace Core.Querying.Find.Extensions.FilterBuilders
{
    public static class CatalogContentBaseFilterBuilderExtensions
    {
        public static ITypeSearch<TEntry> FilterByLanguage<TEntry>(this ITypeSearch<TEntry> typeSearch, IFilterRequest request) where TEntry : IContent
        {
            if (request?.Filters?.Language == null)
            {
                return typeSearch;
            }

            var suffix = request.Filters?.Language.FieldSuffix;
            return typeSearch.FilterOnLanguages(new string[]{suffix});
            //return TypeSearchExtensions.Filter<TEntry>(typeSearch, (TEntry p) => EPiServer.Find.Filters.Match(p.Language.Name, suffix));
        }
    }
}