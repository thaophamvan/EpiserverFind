using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Querying.Extensions;
using Core.Querying.Find.Extensions;
using Core.Querying.Find.Extensions.FilterBuilders;
using Core.Querying.Find.Extensions.QueryBuilders;
using Core.Querying.Find.Extensions.SortBuilders;
using Core.Querying.Find.Helpers;
using Core.Querying.Find.Models.Request;
using Core.Querying.Find.Models.Response;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.Find;
using EPiServer.Find.Cms;

namespace Core.Querying.Services
{
    public class EpiserverFindServices : FilterBase, ISearchServices
    {
        public SearchResponse<TEntry> GenericSearch<TEntry>(ISearchRequest request) where TEntry : IContent
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
            var result = typeSearch.GetContentResultSafe();

            return new SearchResponse<TEntry>
            {
                Items = result.ToPagedList(request, result.TotalMatching),
                Facets = result.ExtractFacet(request.Facets)
            };
        }
    }
}
