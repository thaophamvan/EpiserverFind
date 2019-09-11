using System.Linq.Expressions;

namespace Core.Querying.ExpressionBuilder.Interfaces
{
    internal interface IBuilderHelper
    {
        Expression GetMemberExpression(Expression param, string propertyName);
    }
}