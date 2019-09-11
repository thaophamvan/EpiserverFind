using System.Collections.Generic;
using Core.Querying.ExpressionBuilder.Models;
using Core.Querying.ExpressionBuilder.Models.Request;

namespace Core.Querying.ExpressionBuilder.Interfaces
{
    public interface ISearchRequest : IFilterStatementRequest, ISortRequest, IFacetRequest, IPagedRequest, IMarketRequest
    {
        bool UseWildCardSearch { get; }

        string SearchTerm { get; }

        string FilterSearchTerm { get; set; }

        IList<KeyValuePair<string, double?>> SearchTermFields { get; }

    }

    public interface IFilterStatementRequest
    {
        FilterStatementRequestSpecification Filters { get; }
    }

    public interface IMarketRequest
    {
        string MarketId { get; }
    }

    public interface ISortRequest
    {
        SortSpecification Sorts { get; }
    }

    public interface IFacetRequest
    {
        FacetSpecification Facets { get; }
    }

    public interface IPagedRequest
    {
        int PageNumber { get; }
        int PageSize { get; }
    }
}