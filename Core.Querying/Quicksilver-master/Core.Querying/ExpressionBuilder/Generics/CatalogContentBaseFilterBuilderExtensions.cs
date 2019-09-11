using Core.Querying.ExpressionBuilder.Interfaces;
using EPiServer.Core;
using EPiServer.Find;
using EPiServer.Find.Cms;

namespace Core.Querying.ExpressionBuilder.Generics
{
    public static class CatalogContentBaseFilterBuilderExtensions
    {
        public static ITypeSearch<TEntry> FilterByLanguage<TEntry>(this ITypeSearch<TEntry> typeSearch, IFilterStatementRequest request) where TEntry : IContent
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