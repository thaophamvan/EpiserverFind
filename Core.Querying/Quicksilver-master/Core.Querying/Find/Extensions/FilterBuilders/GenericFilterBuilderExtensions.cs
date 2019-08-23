using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Core.Querying.Find.Extensions.FacetBuilders;
using Core.Querying.Find.Helpers;
using Core.Querying.Find.Models.Request;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.Find;
using EPiServer.Find.Api.Querying;
using EPiServer.Find.Framework;
using EPiServer.ServiceLocation;
using Mediachase.Commerce;

namespace Core.Querying.Find.Extensions.FilterBuilders
{
    public static class GenericFilterBuilderExtensions
    {
        public static ITypeSearch<T> Filter<T>(this ITypeSearch<T> query, IFilterRequest request) where T : IContent
        {
            if (request?.Filters?.Items == null) return query;

            foreach (var sort in (from item in request.Filters.Items
                                  where !MatchFilter(ref query, item)
                                  select item))
            {
                throw new NotImplementedException(string.Format("Type {0} is not implemented for sort by!", sort.GetType()));
            }

            return query;
        }

        private static bool MatchFilter<T>(ref ITypeSearch<T> query, object item) where T : IContent
        {
            var filterBuilder = ClientExtensions.BuildFilter<T>(SearchClient.Instance);
            var inRangeFilter = item as InRangeFilterItem;
            if (inRangeFilter != null)
            {
                //if (inRangeFilter.Field == FacetBuilderExtensions.PreDefineFacetField.PRICE)
                //{
                //    var productFilterBuilder = filterBuilder as FilterBuilder<ProductContent>;
                    
                //    if (productFilterBuilder != null)
                //    {
                //        var currentMarketId = ServiceLocator.Current.GetInstance<ICurrentMarket>().GetCurrentMarket().MarketId.Value;
                //        var currencyCode = ServiceLocator.Current.GetInstance<ICurrencyService>().GetCurrentCurrency().CurrencyCode;
                //        productFilterBuilder = inRangeFilter.Values.Aggregate(productFilterBuilder, (builder, value) =>
                //        {
                //            return builder.Or(x => x.Prices.MatchItem(p => 
                //                p.Price.InRange(value.From, value.To) & 
                //                p.MarketId.Match(currentMarketId) & 
                //                p.CurrencyCode.Match(currencyCode)));
                //        });
                //        query = TypeSearchExtensions.Filter(query, productFilterBuilder);
                //    }
                //}
                //else
                //{
                    filterBuilder = inRangeFilter.Values.Aggregate(filterBuilder, (builder, value) =>
                    {
                        return builder.Or(BuildRangeExpression<T>(inRangeFilter.Field, value.From, value.To));
                    });
                    query = TypeSearchExtensions.Filter(query, filterBuilder);
                //}
                return true;
            }

            var filter = item as MatchFilterItem;
            if (filter == null)
            {
                return false;
            }

            if (filter.Value == null) return true;

            var isNot = item is NotMatchFilterItem;

            filterBuilder = filter.Value.Aggregate(filterBuilder, (builder, value) =>
            {
                if (isNot)
                {
                    return builder.And(BuildMatchExpression<T>(filter.Field, value, true));
                }
                else
                {
                    var expression = BuildMatchExpression<T>(filter.Field, value);
                    if (expression == null)
                    {
                        expression = BuildSpecificMatchExpression<T>(filter.Field, value);
                    }
                    return expression == null ? builder : builder.Or(expression);
                }
            });
            
            if (filterBuilder.HasFilter)
            {
                query = TypeSearchExtensions.Filter(query, filterBuilder);
            }

            return true;
        }

        private static Expression<Func<T, Filter>> BuildSpecificMatchExpression<T>(string field, object compareValue) where T : IContent
        {
            var productTypeToCompare = AssemblyHelper.GetProductTypeWithProperty(field);
            if (productTypeToCompare == null)
            {
                return null;
            }

            var tcompareType = compareValue.GetType();

            var methodInfo = typeof(EPiServer.Find.Filters).GetMethod("Match", BindingFlags.Static | BindingFlags.Public, null, new Type[]
            {
                tcompareType,
                tcompareType
            }, null);

            var parameterExpression = Expression.Parameter(typeof(T), "p");
            var cast = Expression.TypeAs(parameterExpression, productTypeToCompare);
            var value = Expression.Property(cast, field);

            Expression expression = Expression.Call(methodInfo, value, Expression.Constant(compareValue));

            return Expression.Lambda<Func<T, Filter>>(expression, new ParameterExpression[]
            {
                parameterExpression
            });
        }

        private static Expression<Func<T, Filter>> BuildRangeExpression<T>(string propertyName, object fromValue, object toValue) where T: IContent
        {
            var propInfo = PropertyInfoHelper.GetPropertyByName<T>(propertyName);
            if (propInfo == null)
            {
                return null;
            }

            var tcompareType = propInfo.PropertyType;
            if (propInfo.PropertyType.IsGenericType)
            {
                tcompareType = propInfo.PropertyType.GetGenericArguments()[0];
            }

            var methodInfo = typeof(EPiServer.Find.Filters).GetMethod("InRange", BindingFlags.Static | BindingFlags.Public, null, new Type[]{
                propInfo.PropertyType,
                tcompareType,
                tcompareType
            }, null);

            var parameterExpression = Expression.Parameter(typeof(T), "p");
            var value = Expression.Property(parameterExpression, propertyName);
            Expression expression = Expression.Call(methodInfo, value, Expression.Constant(fromValue), Expression.Constant(toValue));

            return Expression.Lambda<Func<T, Filter>>(expression, new ParameterExpression[]
            {
                parameterExpression
            });
        }

        private static Expression<Func<T, Filter>> BuildMatchExpression<T>(string propertyName, object compareValue, bool isNot = false) where T : IContent
        {
            var propInfo = PropertyInfoHelper.GetPropertyByName<T>(propertyName);
            if (propInfo == null) return null;

            var tcompareType = propInfo.PropertyType;
            if (propInfo.PropertyType.IsGenericType)
            {
                tcompareType = propInfo.PropertyType.GetGenericArguments()[0]; 
            }

            var methodInfo = typeof(EPiServer.Find.Filters).GetMethod("Match", BindingFlags.Static | BindingFlags.Public, null, new Type[]
            {
                propInfo.PropertyType,
                tcompareType
            }, null);

            var parameterExpression = Expression.Parameter(typeof(T), "p");
            var value = Expression.Property(parameterExpression, propertyName);

            Expression expression = Expression.Call(methodInfo, value, Expression.Constant(compareValue));
            if (isNot)
            {
                // TODO: NOT WORK YET
                expression = Expression.Not(expression);
            }

            return Expression.Lambda<Func<T, Filter>>(expression, new ParameterExpression[]
            {
                parameterExpression
            });


            throw new NotImplementedException("Value Type must not be null");
        }
    }
}