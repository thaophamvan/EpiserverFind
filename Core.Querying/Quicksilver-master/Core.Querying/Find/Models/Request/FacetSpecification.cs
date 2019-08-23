using System.Collections.Generic;
using EPiServer.Find.Api.Facets;

namespace Core.Querying.Find.Models.Request
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
        public string Field { get; set; }
        public string DataType { get; set; }
        public FacetItem()
        {
            DataType = "string";
        }
    }

    public class DateFacetItem : FacetItem
    {
        public IList<DateRange> Range { get; set; }
    }

    public class NumericFacetItem : FacetItem
    {
        public IList<NumericRange> Range { get; set; }
    }
}