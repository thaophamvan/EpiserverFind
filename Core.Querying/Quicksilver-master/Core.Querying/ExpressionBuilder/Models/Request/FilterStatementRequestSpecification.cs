using System;
using System.Collections.Generic;
using System.Globalization;
using Core.Querying.ExpressionBuilder.Common;
using EPiServer.Find;

namespace Core.Querying.ExpressionBuilder.Models.Request
{
	/// <summary>
	/// Defines how a property should be filtered.
	/// </summary>
	public class FilterStatementRequestSpecification
    {
        public IList<FilterStatementItem> Items { get; set; }

        public CultureInfo CultureInfo { get; set; }

        public Language Language
        {
            get
            {
                if (this.CultureInfo != null)
                {
                    return new Language(this.CultureInfo.EnglishName, this.CultureInfo.EnglishName.ToLower(), this.CultureInfo.Name.ToLower(), "porter", null);
                }
                return null;
            }
        }

        public FilterStatementRequestSpecification()
        {
            Items = new List<FilterStatementItem>();
        }

        public void Add(FilterStatementItem item)
        {
            this.Items.Add(item);
        }

    }

    public class FilterStatementItem
    {
        public FilterStatementItem()
        {
            Parameters = new List<DynamicParameter>();
        }
        /// <summary>
        /// Establishes how this filter statement will connect to the next one. 
        /// </summary>
        public FilterStatementConnector Connector { get; set; }
        /// <summary>
        /// Property identifier conventionalized by for the Expression Builder.
        /// </summary>
        public string PropertyId { get; set; }

        /// <summary>
        /// Property identifier conventionalized by for the Expression Builder.
        /// </summary>
        public Type PropertyType { get; set; }

        /// <summary>
        /// Express the interaction between the property and the constant value defined in this filter statement.
        /// </summary>
        public FilterOperation OperationValue { get; set; }
        /// <summary>
        /// Constant value that will interact with the property defined in this filter statement.
        /// </summary>
        public IList<DynamicParameter> Parameters { get; set; }
        
    }
}