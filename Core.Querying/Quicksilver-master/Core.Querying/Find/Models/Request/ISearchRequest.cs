using System.Collections.Generic;

namespace Core.Querying.Find.Models.Request
{
    public interface ISearchRequest : IFilterRequest, ISortRequest, IFacetRequest, IPagedRequest
    {
        string MarketId { get; }

        bool UseWildCardSearch { get; }

        string SearchTerm { get; }

        string FilterSearchTerm { get; set; }

        IList<KeyValuePair<string, double?>> SearchTermFields { get; }

        bool ShowOnlyClearance { get; set; }
    }

    public interface ISortRequest
    {
        SortSpecification Sorts { get; }
    }

    public interface IFilterRequest
    {
        FilterSpecification Filters { get; }
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