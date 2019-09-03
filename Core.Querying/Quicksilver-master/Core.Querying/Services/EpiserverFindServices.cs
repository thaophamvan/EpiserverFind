using System;
using System.Collections.Generic;
using Core.Querying.Extensions;
using Core.Querying.Find.Extensions;
using Core.Querying.Find.Extensions.FilterBuilders;
using Core.Querying.Find.Extensions.QueryBuilders;
using Core.Querying.Find.Extensions.SortBuilders;
using Core.Querying.Find.Helpers;
using Core.Querying.Find.Models.Request;
using Core.Querying.Find.Models.Response;
using Core.Querying.Infrastructure.Configuration;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.Find;
using EPiServer.Find.Cms;

namespace Core.Querying.Services
{
    public class EpiserverFindServices<TContent> : FilterBase, ISearchServices<TContent> where TContent : IContent
    {
        public SearchResponse<TContent> GenericSearch(ISearchRequest request)
        {
            var typeSearch = (request.Filters?.Language == null)
                ? FindClient.Search<TContent>()
                : FindClient.Search<TContent>(request.Filters?.Language);

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

            typeSearch = typeSearch.FilterForVisitor().Filter(request)
                .FilterByLanguage(request)
                .SortBy(request)
                .PagedBy(request);
            var result = typeSearch.GetContentResultSafe(SiteSettings.Instance.FindCacheTimeOutMinutes);

            return new SearchResponse<TContent>
            {
                Items = result.ToPagedList(request, result.TotalMatching),
                Facets = result.ExtractFacet(request.Facets)
            };
        }

        public IEnumerable<ContentReference> ContentReferencesResult(int cacheForSeconds = 60, bool cacheForEditorsAndAdmins = false)
        {
            throw new NotImplementedException();
        }

        public SearchResponse<TContent> FilterBySiteId(string siteId)
        {
            throw new NotImplementedException();
        }

        public SearchResponse<TContent> ChildrenOf(ContentReference parentLink)
        {
            throw new NotImplementedException();
        }

        public SearchResponse<TContent> IncludeWasteBasket()
        {
            throw new NotImplementedException();
        }

        public SearchResponse<TContent> PublishedIgnoreDates()
        {
            throw new NotImplementedException();
        }

        public SearchResponse<TContent> Published()
        {
            throw new NotImplementedException();
        }

        public SearchResponse<TContent> InRemoteSite(string remoteSite)
        {
            throw new NotImplementedException();
        }

        public SearchResponse<TContent> OfType(params Type[] types)
        {
            throw new NotImplementedException();
        }

        public SearchResponse<TContent> FreeTextSearch(string query)
        {
            throw new NotImplementedException();
        }

        public SearchResponse<TContent> BlockSearch<T>() where T : BlockData
        {
            throw new NotImplementedException();
        }

        public SearchResponse<TContent> PageSearch<T>() where T : PageData
        {
            throw new NotImplementedException();
        }

        public SearchResponse<TContent> NodeContentBaseSearch<T>() where T : NodeContentBase
        {
            throw new NotImplementedException();
        }

        public SearchResponse<TContent> EntryContentBaseSearch<T>() where T : EntryContentBase
        {
            throw new NotImplementedException();
        }

        public SearchResponse<TContent> VariantSearch<T>() where T : VariationContent
        {
            throw new NotImplementedException();
        }

        public SearchResponse<TContent> ProductSearch<T>() where T : ProductContent
        {
            throw new NotImplementedException();
        }

        public SearchResponse<TContent> MultiSearch(Func<ITypeSearch<TContent>, ITypeSearch<TContent>> request)
        {
            throw new NotImplementedException();
        }

        public SearchResponse<TContent> MultiUnifiedSearch(Func<ITypeSearch<TContent>, ITypeSearch<TContent>> request)
        {
            throw new NotImplementedException();
        }

        public SearchResponse<TContent> DynamicMultiSearch(Func<ITypeSearch<TContent>, ITypeSearch<TContent>> request)
        {
            throw new NotImplementedException();
        }

        public SearchResponse<TContent> GenericUnifiedSearch(Func<ITypeSearch<TContent>, ITypeSearch<TContent>> request)
        {
            throw new NotImplementedException();
        }
    }
}
