using System.Collections.Generic;
using Core.Querying.Find.FacetRegistry;
using Core.Querying.Find.Models.Request;
using EPiServer.ServiceLocation;

namespace Core.Querying.Find.SearchRequests
{
    public interface IProductSearchRequest : ISearchRequest
    {
        string WarehouseCode { get; set; }
        int PriceInterval { get; set; }
    }

    public class ProductSearchRequest : SearchRequest, IProductSearchRequest
    {
        public string WarehouseCode { get; set; }
        public int PriceInterval { get; set; }

        public ProductSearchRequest(string marketId, IList<object> rootId = null)
        {
            MarketId = marketId;
            Filters.Add(new MatchFilterItem("Parents", rootId));
            Facets = ServiceLocator.Current.GetInstance<IFacetRegistry>().Facets;

            PageSize = 10;
            PageNumber = 1;
        }
    }
}