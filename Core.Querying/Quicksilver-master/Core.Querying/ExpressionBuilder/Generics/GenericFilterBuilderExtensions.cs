using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Core.Querying.ExpressionBuilder.Common;
using Core.Querying.ExpressionBuilder.Interfaces;
using Core.Querying.ExpressionBuilder.Models;
using Core.Querying.ExpressionBuilder.Models.Request;
using EPiServer.Core;
using EPiServer.Find;
using EPiServer.Find.Api.Querying;
using EPiServer.Find.Framework;

namespace Core.Querying.ExpressionBuilder.Generics
{
    public static class GenericFilterBuilderExtensions
    {
        public static ITypeSearch<T> Filter<T>(this ITypeSearch<T> query, IFilterStatementRequest request) where T : IContent
        {
            if (request?.Filters?.Items == null) return query;
            var filterBuilder = ClientExtensions.BuildFilter<T>(SearchClient.Instance);

            filterBuilder = request.Filters.Items.Aggregate(filterBuilder, (builder, item) =>
            {
                if (item.Connector == FilterStatementConnector.And)
                {
                    return builder.And(GetExpression<T>(item));
                }
                else
                {
                    var expression = GetExpression<T>(item);
                    if (expression == null)
                    {
                        expression = GetExpression<T>(item);
                    }
                    return expression == null ? builder : builder.Or(expression);
                }
            });

            if (filterBuilder.HasFilter)
            {
                query = TypeSearchExtensions.Filter(query, filterBuilder);
            }

            return query;
        }

        
        private static Expression<Func<T, Filter>> GetExpression<T>(FilterStatementItem item)
            where T : IContent
        {
            var types = new List<Type>(){item.PropertyType};
            types.AddRange(item.Parameters.Select(p=>p.ParameterType));
            var methodInfo = typeof(Filters).GetMethod(item.OperationValue.ToString(),
                BindingFlags.Static | BindingFlags.Public, null, types.ToArray(), null);

            var parameterExpression = Expression.Parameter(typeof(T), "p");
            var value = Expression.Property(parameterExpression, item.PropertyId);
            List<Expression> exp = new List<Expression>(){value};
            exp.AddRange(item.Parameters.Select(i => Expression.Constant(i.Value)));
            Expression expression = Expression.Call(methodInfo, exp.ToArray());

            return Expression.Lambda<Func<T, Filter>>(expression, new ParameterExpression[]
            {
                parameterExpression
            });
        }
    }
}

