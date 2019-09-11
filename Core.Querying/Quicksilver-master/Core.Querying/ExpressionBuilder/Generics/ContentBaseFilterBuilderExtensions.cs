using System.Collections.Generic;
using Core.Querying.ExpressionBuilder.Interfaces;
using EPiServer.Core;
using EPiServer.Find;
using EPiServer.Find.Cms;

namespace Core.Querying.ExpressionBuilder.Generics
{
    public static class ContentBaseFilterBuilderExtensions
    {
        public static ITypeSearch<TEntry> IncludeContentUnder<TEntry>(this ITypeSearch<TEntry> query, IList<string> list) where TEntry : IContent
        {
            if (list == null) return query;

            return query.Filter(q => q.Ancestors().In(list));
        }
  
        public static ITypeSearch<TEntry> ExcludeContentUnder<TEntry>(this ITypeSearch<TEntry> query, IList<string> list) where TEntry : IContent
        {
            if (list == null) return query;

            return query.Filter(q => !q.Ancestors().In(list));
        }

        public static ITypeSearch<TEntry> PagedBy<TEntry>(this ITypeSearch<TEntry> typeSearch, IPagedRequest request) where TEntry : IContent
        {
            var pageIndex = request.PageNumber < 1 ? 0 : request.PageNumber - 1;
            var pageSize = request.PageSize > 1000 ? 1000 : request.PageSize;
       
            var take = pageSize;
            var skip = pageSize * pageIndex;

            typeSearch = SearchExtensions.Take(SearchExtensions.Skip(typeSearch, skip), take);

            return typeSearch;
        }
    }
}