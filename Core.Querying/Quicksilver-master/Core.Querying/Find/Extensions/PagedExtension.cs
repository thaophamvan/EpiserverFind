using System.Collections.Generic;
using Core.Querying.Find.Models.Request;
using Core.Querying.Find.Models.Response;

namespace Core.Querying.Find.Extensions
{
    public static class PagedExtension
    {
        public static IPagedList<T> ToPagedList<T>(this IEnumerable<T> subSet, IPagedRequest request, int totalItemCount)
        {
            return new PagedList<T>(subSet, request.PageNumber, request.PageSize, totalItemCount);
        }

        public static IPagedList<T> ToPagedList<T>(this IEnumerable<T> subSet)
        {
            return new PagedList<T>(subSet);
        }
    }
}