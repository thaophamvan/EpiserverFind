using System;
using System.Collections.Generic;
using Core.Querying.Find.Extensions;
using Core.Querying.Find.Helpers;
using Core.Querying.Find.Interfaces;
using Core.Querying.Find.Models.Request;
using Core.Querying.Find.Models.Response;
using Core.Querying.Find.SearchRequests;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.Find;
using EPiServer.Find.Cms;
using EPiServer.Find.Framework;

namespace Core.Querying.Find.Services
{
    public class SearchService : ISearchService
    {
        private readonly IContentDataQuery<IContent> _contentDataQuery;

        public SearchService(ContentDataQuery<IContent> contentDataQuery)
        {
            _contentDataQuery = contentDataQuery;
        }

        public virtual SearchResponse<TEntry> SearchProduct<TEntry>(IProductSearchRequest request) where TEntry : ProductContent
        {
            ValidateRequest(request);

            var result = _contentDataQuery.GeneralSearch<TEntry>(request).GetContentResult();

            return new SearchResponse<TEntry>
            {
                Items = result.ToPagedList(request, result.TotalMatching),
                Facets = result.ExtractFacet(request.Facets, request.PriceInterval)
            };
        }

        public IEnumerable<ProductContent> GetRelatedProducts(ProductContent currentProduct, string higherLevelId)
        {
            throw new NotImplementedException();
        }

        protected virtual void ValidateRequest(ISearchRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (string.IsNullOrWhiteSpace(request.MarketId))
            {
                throw new ArgumentNullException("request.MarketId");
            }
        }
    }
}
