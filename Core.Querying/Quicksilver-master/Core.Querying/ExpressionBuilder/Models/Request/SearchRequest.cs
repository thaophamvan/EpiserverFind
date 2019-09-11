using System.Collections.Generic;
using ISearchRequest = Core.Querying.ExpressionBuilder.Interfaces.ISearchRequest;

namespace Core.Querying.ExpressionBuilder.Models.Request
{
    public class SearchRequest : ISearchRequest
    {
        public string MarketId { get; set; }

        public bool UseWildCardSearch { get; set; }

        public string SearchTerm { get; set; }

        public string FilterSearchTerm { get; set; }
        
        public IList<KeyValuePair<string, double?>> SearchTermFields { get; set; }

        public FilterStatementRequestSpecification Filters { get; }

        public SortSpecification Sorts { get; set; }

        public FacetSpecification Facets { get; set; }

        /// <summary>
        /// 1 based page number
        /// </summary>
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public SearchRequest()
        {
            Filters = new FilterStatementRequestSpecification();
            Facets = new FacetSpecification();
            Sorts = new SortSpecification();
        }
    }
}