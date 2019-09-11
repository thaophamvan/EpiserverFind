using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Core.Querying.ExpressionBuilder.Attributes
{
    internal class NumberOfValuesAttribute : Attribute
    {
        [Range(0, 4, ErrorMessage = "Operations may only have from none to four values.")]
        [DefaultValue(1)]
        public int NumberOfValues { get; private set; }

        /// <summary>
        /// Defines the number of values supported by the operation.
        /// </summary>
        /// <param name="numberOfValues">Number of values the operation demands.</param>
        public NumberOfValuesAttribute(int numberOfValues = 1)
        {
            NumberOfValues = numberOfValues;
        }
    }
}
