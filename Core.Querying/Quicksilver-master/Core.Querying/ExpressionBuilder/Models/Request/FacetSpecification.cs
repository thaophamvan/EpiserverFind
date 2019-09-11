using System;
using System.Collections.Generic;
using Core.Querying.ExpressionBuilder.Common;
using EPiServer.Find.Api.Facets;

namespace Core.Querying.ExpressionBuilder.Models.Request
{
    public class FacetSpecification
    {
        public IList<FacetItem> Items { get; set; }

        public FacetSpecification()
        {
            this.Items = new List<FacetItem>();
        }

        public void Add(FacetItem item)
        {
            this.Items.Add(item);
        }
    }

    public class FacetItem
    {
        /// <summary>
        /// Field name, is also to be facet name.
        /// </summary>
        public string PropertyId { get; set; }
        public Type PropertyType { get; set; }
        public FacetOperation OperationValue { get; set; }
        public object Interval { get; set; }
        public FacetItem()
        {
            PropertyType = typeof(string);
        }
    }
}