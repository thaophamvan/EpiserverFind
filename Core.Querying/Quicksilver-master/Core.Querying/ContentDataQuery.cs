using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Web.UI.WebControls;
using Core.Querying.Extensions;
using Core.Querying.Find.Extensions.FilterBuilders;
using Core.Querying.Find.Extensions.QueryBuilders;
using Core.Querying.Find.Extensions.SortBuilders;
using Core.Querying.Find.Models.Request;
using EPiCode.DynamicMultiSearch;
using EPiServer.Cms.Shell;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.Find;
using EPiServer.Find.Api.Querying;
using EPiServer.Find.Cms;
using EPiServer.Find.Commerce;
using EPiServer.Find.Framework;
using EPiServer.Find.Helpers;
using EPiServer.Find.UnifiedSearch;
using EPiServer.Security;

namespace Core.Querying
{
    /// <summary>
    /// Provides functionality to evaluate queries against a ContentData.
    /// </summary>
    /// <typeparam name="TContentData">
    /// The type of the ContentData to query.
    /// That is, you can use either the type you specified or any type 
    /// that is more derived.
    /// </typeparam>
    public class ContentDataQuery<TContentData> : FilterBase, IContentDataQuery<TContentData> where TContentData : IContent
    {
        public IEnumerable<ContentReference> UnCachedContentReferencesResult(int cacheForSeconds = 60, bool cacheForEditorsAndAdmins = false)
        {
            var typeSearch = ApplyFilterBaseContent<TContentData>();
            var results = typeSearch.GetContentResultSafe(cacheForSeconds, cacheForEditorsAndAdmins).Items;
            return results.Select(i => i.ContentLink);
        }

        public IEnumerable<ContentReference> ContentReferencesResult(int cacheForSeconds = 60, bool cacheForEditorsAndAdmins = false)
        {
            var typeSearch = ApplyFilterBaseContent<TContentData>();
            var results = typeSearch.GetContentResultSafe(cacheForSeconds, cacheForEditorsAndAdmins).Items;
            return results.Select(i => i.ContentLink);
        }

        public ITypeSearch<TContentData> FilterBySiteId(string siteId)
        {
            throw new NotImplementedException();
        }

        public ITypeSearch<TContentData> ChildrenOf(ContentReference parentLink)
        {
            var typeSearch = ApplyFilterBaseContent<TContentData>();
            var results = typeSearch.Filter(p => p.ParentLink.Match(parentLink));
            return results;
        }

        public ITypeSearch<TContentData> IncludeWasteBasket()
        {
            var typeSearch = ApplyFilterBaseContent<TContentData>();
            var results = typeSearch.Filter(p => p.IsDeleted.Match(true));
            return results;
        }

        public ITypeSearch<TContentData> PublishedIgnoreDates()
        {
            var results = FindClient.Search<TContentData>().Filter(p=>(p as IVersionable).IsNotNull().Match(true))
                .Filter(p => (p as IVersionable).Status.Match(VersionStatus.Published));
            return results;
        }

        public ITypeSearch<TContentData> Published()
        {
            var typeSearch = ApplyFilterBaseContent<TContentData>();
            var results = typeSearch.Filter(p => p.IsPublished().Match(true));
            return results;
        }

        public ITypeSearch<TContentData> InRemoteSite(string remoteSite)
        {
            throw new NotImplementedException();
        }

        public ITypeSearch<TContentData> OfType(params Type[] types)
        {
            var typeSearch = ApplyFilterBaseContent<TContentData>();
            var results = typeSearch.FilterByExactTypes(types);
            return results;
        }

        public ITypeSearch<TContentData> FreeTextSearch(string query)
        {
            var typeSearch = ApplyFilterBaseContent<TContentData>();
            var searchResult = typeSearch.For(query);
            return searchResult;
        }

        public ITypeSearch<TBlock> BlockSearch<TBlock>() where TBlock : BlockData
        {
            var typeSearch = ApplyFilterBaseContentData<TBlock>();
            return typeSearch;
        }

        public ITypeSearch<TPage> PageSearch<TPage>() where TPage : PageData
        {
            var typeSearch = ApplyFilterBaseContentData<TPage>();
            return typeSearch;
        }

        public ITypeSearch<TEntry> NodeContentBaseSearch<TEntry>() where TEntry : NodeContentBase
        {
            var typeSearch = ApplyFilterBaseContentData<TEntry>();
            return typeSearch;
        }

        public ITypeSearch<TEntry> EntryContentBaseSearch<TEntry>() where TEntry : EntryContentBase
        {
            var typeSearch = ApplyFilterBaseContentData<TEntry>();
            return typeSearch;
        }

        public ITypeSearch<TEntry> VariantSearch<TEntry>() where TEntry : VariationContent
        {
            var typeSearch = ApplyFilterBaseContentData<TEntry>();
            return typeSearch;
        }

        public ITypeSearch<TEntry> ProductSearch<TEntry>() where TEntry : ProductContent
        {
            var typeSearch = ApplyFilterBaseContentData<TEntry>();
            return typeSearch;
        }

        public ITypeSearch<TEntry> GeneralSearch<TEntry>(ISearchRequest request) where TEntry : CatalogContentBase
        {
            var typeSearch = (request.Filters?.Language == null)
                ? FindClient.Search<TEntry>()
                : FindClient.Search<TEntry>(request.Filters?.Language);

            var searchTerm = string.IsNullOrEmpty(request.SearchTerm) ? request.FilterSearchTerm : $"{request.SearchTerm} {request.FilterSearchTerm}";

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var queriedSearch = typeSearch.For(searchTerm);

                if (request.SearchTermFields != null)
                {
                    foreach (var current3 in request.SearchTermFields)
                    {
                        queriedSearch =
                            QueryStringSearchExtensions.InField(queriedSearch, current3.Key, current3.Value);
                    }
                }

                // TODO: Need to improve to use request.SearchTermFields for XHtmlString field text search
                //var productQueriedSearch = queriedSearch as IQueriedSearch<ProductContent, QueryStringQuery>;
                //if (productQueriedSearch != null)
                //{
                //    productQueriedSearch = productQueriedSearch
                //        .InField(x => x.LongDescription, 4);
                //    queriedSearch = (IQueriedSearch<TEntry, QueryStringQuery>)productQueriedSearch;
                //}

                queriedSearch = queriedSearch.WithAndAsDefaultOperator();
                queriedSearch = queriedSearch.UsingSynonyms();

                typeSearch = queriedSearch;
                typeSearch = typeSearch.ApplyBestBets();

                if (request.UseWildCardSearch)
                {
                    foreach (var term in request.SearchTermFields)
                    {
                        typeSearch = typeSearch.WildCardQuery(request.SearchTerm, term.Key, term.Value);
                    }
                }
            }

            var productQuerySearch = typeSearch as ITypeSearch<ProductContent>;
            //if (productQuerySearch != null)
            //{
            //    var currentWareHouseCode = request is IProductSearchRequest && !string.IsNullOrEmpty((request as IProductSearchRequest).WarehouseCode) ?
            //        (request as IProductSearchRequest).WarehouseCode : _warehouseService.GetCurrentWarehouseCode();

            //    var currency = _currencyService.GetCurrentCurrency();
            //    productQuerySearch = productQuerySearch.Filter(x => x.IsProductAvailability,
            //        i => i.IsAvailable.Match(true) & i.CurrencyCode.Match(currency.CurrencyCode) & i.WarehouseCode.Match(currentWareHouseCode) & i.Market.Match(request.MarketId));

            //    if (request.ShowOnlyClearance)
            //    {
            //        productQuerySearch = productQuerySearch.Filter(x => x.IsProductClearance,
            //            i => i.IsClearance.Match(true) & i.CurrencyCode.Match(currency.CurrencyCode) & i.Market.Match(request.MarketId));
            //    }

            //    typeSearch = (ITypeSearch<TEntry>)productQuerySearch;
            //}
            //TODO move to config file
            int FIND_STATICALLY_CACHE_FOR = 15;
            typeSearch = typeSearch.FilterForVisitor().Filter(request)
                .FilterByLanguage(request)
                .SortBy(request)
                .PagedBy(request)
                .StaticallyCacheFor(TimeSpan.FromMinutes(FIND_STATICALLY_CACHE_FOR));

            return typeSearch;
        }

        public ITypeSearch<TEntry> MultiSearch<TEntry>(Func<ITypeSearch<TEntry>, ITypeSearch<TEntry>> request) where TEntry : ContentData
        {
            throw new NotImplementedException();
        }

        public ITypeSearch<TEntry> MultiUnifiedSearch<TEntry>(Func<ITypeSearch<TEntry>, ITypeSearch<TEntry>> request) where TEntry : ContentData
        {
            throw new NotImplementedException();
        }

        public ITypeSearch<TEntry> DynamicMultiSearch<TEntry>(Func<ITypeSearch<TEntry>, ITypeSearch<TEntry>> request) where TEntry : ContentData
        {
            throw new NotImplementedException();
        }

        public ITypeSearch<ISearchContent> GeneralUnifiedSearch<TEntry>(Func<ITypeSearch<TEntry>, ITypeSearch<TEntry>> request) where TEntry : ContentData
        {
            throw new NotImplementedException();
        }
    }
}
