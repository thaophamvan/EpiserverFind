using System;
using System.Collections.Generic;
using Core.Querying.ExpressionBuilder.Common;

namespace Core.Querying.ExpressionBuilder.Attributes
{
    internal class SupportedOperationsAttribute : Attribute
    {
        public List<FilterOperation> SupportedOperations { get; private set; }

        /// <summary>
        /// Defines operations that are supported by an specific TypeGroup.
        /// </summary>
        /// <param name="supportedOperations">List of supported operations.</param>
        public SupportedOperationsAttribute(params FilterOperation[] supportedOperations)
        {
            SupportedOperations = new List<FilterOperation>(supportedOperations);
        }
    }
}