using System;
using System.Linq.Expressions;
using Core.Querying.ExpressionBuilder.Common;
using Core.Querying.ExpressionBuilder.Interfaces;
using Core.Querying.ExpressionBuilder.Models.Request;
using EPiServer.Core;
using EPiServer.Find;
using EPiServer.Find.Api.Facets;

namespace Core.Querying.ExpressionBuilder.Generics
{
    public static class FacetBuilderExtensions
    {

        public static ITypeSearch<T> AddFacetFor<T>(this ITypeSearch<T> query, IFacetRequest request) where T : IContent
        {
            if (request?.Facets?.Items == null) return query;

            return query
                .BuildFacetFor(request.Facets);
        }

        public static ITypeSearch<T> BuildFacetFor<T>(this ITypeSearch<T> query, FacetSpecification facets) where T : IContent
        {
            if (facets == null) return query;

            foreach (var facet in facets.Items)
            {
                switch (facet.OperationValue)
                {
                    case FacetOperation.TermsFacetFor:
                    {
                        var lambaExpression = GetLamdaExpression<T>(facet);
                        query = TypeSearchExtensions.TermsFacetFor(query, lambaExpression);
                        break;
                    }
                    case FacetOperation.RangeFacetFor:
                    {
                        if (facet.PropertyType.Name == "DateTime")
                        {
                            var lambaExpression = GetLamdaExpression<T, DateTime>(facet);
                            query = TypeSearchExtensions.RangeFacetFor(query, lambaExpression);
                        }
                        else
                        {
                            var lambaExpression = GetLamdaExpression<T, object>(facet);
                            query = TypeSearchExtensions.RangeFacetFor(query, lambaExpression);
                        }
                       
                        break;
                    }
                    case FacetOperation.HistogramFacetFor:
                    {
                        if (facet.PropertyType.Name == "DateTime")
                        {
                            var lambaExpression = GetLamdaExpression<T, DateTime>(facet);
                            query = TypeSearchExtensions.HistogramFacetFor(query, lambaExpression, (DateInterval)facet.Interval);
                        }
                        else
                        {
                            var lambaExpression = GetLamdaExpression<T, object>(facet);
                            query = TypeSearchExtensions.HistogramFacetFor(query, lambaExpression, (int)facet.Interval);
                        }

                        break;
                    }
                    default: break;
                }
               
            }

            return query;
        }


        private static dynamic GetLamdaExpression<T>(FacetItem item) where T : IContent
        {
            return item.PropertyId.GetLamdaExpression<T>(item.PropertyType);
        }

        private static dynamic GetLamdaExpression<T, TProp>(FacetItem item) where T : IContent
        {
            
            var expression = item.PropertyId.GetLamdaExpression<T>(item.PropertyType);

            // Add the boxing operation, but get a weakly typed expression
            var converted = Expression.Convert
                (expression.Body, typeof(object));

            // Use Expression.Lambda to get back to strong typing
            return Expression.Lambda<Func<T, TProp>>
                (converted, expression.Parameters);
        }

        private static dynamic GetLamdaExpression<T>(this string propertyId, Type type) where T : IContentData
        {
            ParameterExpression expParam = Expression.Parameter(typeof(T), "x");
            MemberExpression expProp = Expression.Property(expParam, propertyId);
            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            return (dynamic)Expression.Lambda(delegateType, expProp, expParam);
        }

    }
}