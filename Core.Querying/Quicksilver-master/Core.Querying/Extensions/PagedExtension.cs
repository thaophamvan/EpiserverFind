using System.Collections.Generic;
using Core.Querying.ExpressionBuilder.Interfaces;
using Core.Querying.ExpressionBuilder.Models.Response;

namespace Core.Querying.Extensions
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