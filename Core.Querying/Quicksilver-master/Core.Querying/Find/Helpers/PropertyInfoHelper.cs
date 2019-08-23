using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using EPiServer.Core;

namespace Core.Querying.Find.Helpers
{
    public static class PropertyInfoHelper
    {
        public static PropertyInfo GetPropertyByName<T>(string name) where T : IContent
        {
            return typeof(T).GetProperties().FirstOrDefault(p => p.Name == name);
        }

        public static dynamic GetLamdaExpression<T>(string name) where T : IContent
        {
            var propertyInfo = GetPropertyByName<T>(name);
            if (propertyInfo == null)
            {
                return null;
            }
            return propertyInfo.GetLamdaExpression<T>();
        }

        public static dynamic GetLamdaExpression<T, TProp>(string name) where T : IContent
        {
            var propertyInfo = GetPropertyByName<T>(name);
            var expression = propertyInfo.GetLamdaExpression<T>();

            // Add the boxing operation, but get a weakly typed expression
            var converted = Expression.Convert
                 (expression.Body, typeof(object));

            // Use Expression.Lambda to get back to strong typing
            return Expression.Lambda<Func<T, TProp>>
                 (converted, expression.Parameters);
        }

        private static dynamic GetLamdaExpression<T>(this PropertyInfo propertyInfo) where T : IContent
        {
            ParameterExpression expParam = Expression.Parameter(typeof(T), "x");
            MemberExpression expProp = Expression.Property(expParam, propertyInfo);
            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), propertyInfo.PropertyType);
            return (dynamic)Expression.Lambda(delegateType, expProp, expParam);
        }
    }
}