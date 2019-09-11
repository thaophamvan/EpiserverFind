using System.Collections.Generic;
using System.Linq;
using Core.Querying.ExpressionBuilder.Models.Response;
using EPiServer.Find;
using EPiServer.Find.Api;

namespace Core.Querying
{
    public static class EmptySearchResultsFactory
    {
        public static SearchResults<T> CreateSearchResults<T>()
        {
            return new SearchResults<T>(CreateSearchResult<T>());
        }

        public static SearchResult<T> CreateSearchResult<T>()
        {
            return new SearchResult<T>()
            {
                Facets = new FacetResults(),

                Hits = new HitCollection<T>
                {
                    Hits = Enumerable.Empty<SearchHit<T>>().ToList(),
                    Total = 0
                },
                Shards = new Shards()
            };
        }

        public static SearchResponse<T> CreateSearchResponse<T>()
        {
            return new SearchResponse<T>()
            {
                Facets = new List<FacetGroup>(),
                Items = new PagedList<T>(Enumerable.Empty<T>(), 0,0,0)
            };
        }
    }
}