﻿using System;
using System.Collections.Generic;
using System.Linq;
using Core.Querying.ExpressionBuilder.Generics;
using Core.Querying.ExpressionBuilder.Helpers;
using Core.Querying.ExpressionBuilder.Interfaces;
using Core.Querying.ExpressionBuilder.Models.Response;
using Core.Querying.Extensions;
using Core.Querying.Infrastructure.Configuration;
using EPiServer.Core;
using EPiServer.Find;
using EPiServer.Find.Cms;
using EPiServer.Find.Helpers;
using SearchSortBuilderExtensions = Core.Querying.ExpressionBuilder.Generics.SearchSortBuilderExtensions;
using WildCardQueryBuilderExtensions = Core.Querying.ExpressionBuilder.Generics.WildCardQueryBuilderExtensions;

namespace Core.Querying.Services
{
    public class EpiserverFindServices<TContent> : FilterBase, ISearchServices<TContent> where TContent : IContent
    {
        private ITypeSearch<TContent> CreateBaseContentSearch(ISearchRequest request)
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
                        typeSearch = WildCardQueryBuilderExtensions.WildCardQuery(typeSearch, request.SearchTerm, term.Key, term.Value);
                    }
                }
            }

            typeSearch = SearchSortBuilderExtensions.SortBy(typeSearch.FilterForVisitor().Filter(request)
                    .FilterByLanguage(request), request)
                .PagedBy(request);
            return typeSearch;
        }
        public SearchResponse<TContent> GenericSearch(ISearchRequest request)
        {
            //var typeSearch = CreateBaseContentSearch(request).AddFacetFor(request);
            //var results = typeSearch.GetContentResultSafe(SiteSettings.Instance.FindCacheTimeOutMinutes);

            //return new SearchResponse<TContent>
            //{
            //    Items = results.ToPagedList(request, results.TotalMatching),
            //    Facets = results.ExtractFacet(request.Facets)
            //};
            return null;
        }

        public IEnumerable<ContentReference> ContentReferencesResult(int cacheForSeconds = 60, bool cacheForEditorsAndAdmins = false)
        {
            var typeSearch = ApplyFilterBaseContent<TContent>();
            var results = typeSearch.GetContentResultSafe(cacheForSeconds, cacheForEditorsAndAdmins).Items;
            return results.Select(i => i.ContentLink);
        }

        public SearchResponse<TContent> FilterBySiteId(string siteId)
        {
            throw new NotImplementedException();
        }

        public SearchResponse<TContent> ChildrenOf(ContentReference parentLink)
        {
            var typeSearch = ApplyFilterBaseContent<TContent>();
            var results = typeSearch.Filter(p => p.ParentLink.Match(parentLink)).GetContentResultSafe();
            return new SearchResponse<TContent>
            {
                Items = results.ToPagedList()
            };
        }

        public SearchResponse<TContent> IncludeWasteBasket()
        {
            var typeSearch = ApplyFilterBaseContent<TContent>();
            var results = typeSearch.Filter(p => p.IsDeleted.Match(true)).GetContentResultSafe();
            return new SearchResponse<TContent>
            {
                Items = results.ToPagedList()
            };
        }

        public SearchResponse<TContent> PublishedIgnoreDates()
        {
            var results = FindClient.Search<TContent>().Filter(p => (p as IVersionable).IsNotNull().Match(true))
                .Filter(p => (p as IVersionable).Status.Match(VersionStatus.Published)).GetContentResultSafe();
            return new SearchResponse<TContent>
            {
                Items = results.ToPagedList()
            };
        }

        public SearchResponse<TContent> Published()
        {
            var results = ApplyFilterBaseContent<TContent>().GetContentResultSafe();
            return new SearchResponse<TContent>
            {
                Items = results.ToPagedList()
            };
        }

        public SearchResponse<TContent> OfType(params Type[] types)
        {
            var typeSearch = ApplyFilterBaseContent<TContent>();
            var results = typeSearch.FilterByExactTypes(types).GetContentResultSafe();
            return new SearchResponse<TContent>
            {
                Items = results.ToPagedList()
            };
        }

        public SearchResponse<TContent> FreeTextSearch(string query, int maxItemNumber)
        {
            var typeSearch = ApplyFilterBaseContent<TContent>().Take(maxItemNumber);
            var results = typeSearch.For(query).GetContentResultSafe();

            return new SearchResponse<TContent>
            {
                Items = results.ToPagedList()
            };
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
