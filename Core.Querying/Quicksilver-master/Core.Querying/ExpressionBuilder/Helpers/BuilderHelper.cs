using System;
using System.Linq.Expressions;
using System.Reflection;
using Core.Querying.ExpressionBuilder.Interfaces;

namespace Core.Querying.ExpressionBuilder.Helpers
{
	internal class BuilderHelper : IBuilderHelper
	{
        public readonly MethodInfo TrimMethod = typeof(string).GetMethod("Trim", new Type[0]);
        public readonly MethodInfo ToLowerMethod = typeof(string).GetMethod("ToLower", new Type[0]);

        public Expression GetMemberExpression(Expression param, string propertyName)
        {
        	if (propertyName.Contains("."))
        	{
        		int index = propertyName.IndexOf(".");
        		var subParam = Expression.Property(param, propertyName.Substring(0, index));
        		return GetMemberExpression(subParam, propertyName.Substring(index + 1));
        	}
            
            return Expression.Property(param, propertyName);
        }
	}
}