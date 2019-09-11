using System;

namespace Core.Querying.ExpressionBuilder.Models
{
    public class DynamicParameter
    {
        public Type ParameterType { get; set; }
        public object Value { get; set; }
    }
}
