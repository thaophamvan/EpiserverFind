using System;
using System.Linq;
using Core.Querying.ExpressionBuilder.Helpers;
using Core.Querying.ExpressionBuilder.Interfaces;
using Core.Querying.ExpressionBuilder.Models.Request;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.Find;

namespace Core.Querying.ExpressionBuilder.Generics
{
    public static class SearchSortBuilderExtensions
    {
        public static ITypeSearch<T> SortBy<T>(this ITypeSearch<T> query, ISortRequest request) where T : IContent
        {
            if (request?.Sorts?.Items == null) return query;

            //foreach (var sort in (from item in request.Sorts.Items
            //                      where !OrderBy(ref query, item)
            //                      select item))
            //{
            //    throw new NotImplementedException(string.Format("Type {0} is not implemented for sort by!", sort.GetType()));
            //}

            return query;
        }

        private static bool OrderBy<T>(ref ITypeSearch<T> query, object item) where T : ProductContent
        {
            var searchSort = item as SortItem;
            if (searchSort == null)
            {
                return false;
            }

            var expression = PropertyInfoHelper.GetLamdaExpression<T>(searchSort.Field);

            query = (searchSort.Ascending
                ? TypeSearchExtensions.OrderBy(query, expression)
                : TypeSearchExtensions.OrderByDescending(query, expression));

            return true;
        }
    }
}