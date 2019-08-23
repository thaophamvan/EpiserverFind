using System.Collections.Generic;

namespace Core.Querying.Find.Models.Request
{
    public class SearchRequest : ISearchRequest
    {
        public string MarketId { get; set; }
        
        public bool UseWildCardSearch { get; set; }

        public string SearchTerm { get; set; }

        public string FilterSearchTerm { get; set; }
        
        public IList<KeyValuePair<string, double?>> SearchTermFields { get; set; }

        public bool ShowOnlyClearance { get; set; }

        public SortSpecification Sorts { get; set; }

        public FilterSpecification Filters { get; set; }

        public FacetSpecification Facets { get; set; }

        /// <summary>
        /// 1 based page number
        /// </summary>
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public SearchRequest()
        {
            Filters = new FilterSpecification();
            Facets = new FacetSpecification();
            Sorts = new SortSpecification();
        }
    }
}