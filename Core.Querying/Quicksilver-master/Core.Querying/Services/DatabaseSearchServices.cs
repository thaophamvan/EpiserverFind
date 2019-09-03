using System;
using System.Collections.Generic;
using Core.Querying.Find.Models.Request;
using Core.Querying.Find.Models.Response;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.Find;

namespace Core.Querying.Services
{
    public class DatabaseSearchServices<TContent> : ISearchServices<TContent> where TContent : IContent
    {
        public SearchResponse<TContent> GenericSearch(ISearchRequest request)
        {
            throw new NotImplementedException();
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

        public SearchResponse<TContent> FreeTextSearch(string query, int maxItemNumber)
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
