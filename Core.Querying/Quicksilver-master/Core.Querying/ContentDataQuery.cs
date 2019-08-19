using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Web.UI.WebControls;
using Core.Querying.Extensions;
using EPiCode.DynamicMultiSearch;
using EPiServer.Cms.Shell;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.Find;
using EPiServer.Find.Api.Querying;
using EPiServer.Find.Cms;
using EPiServer.Find.Framework;
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
    public class ContentDataQuery<TContentData> : IContentDataQuery<TContentData> where TContentData : IContent
    {
        private readonly IClient _searchClient = ContentDataQueryHandler.Instance.Create();
        public IEnumerable<PageReference> UnCachedContentReferencesResult()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ContentReference> ContentReferencesResult(int cacheForSeconds = 60, bool cacheForEditorsAndAdmins = false)
        {
            var results = _searchClient.Search<TContentData>().GetContentResultSafe(cacheForSeconds, cacheForEditorsAndAdmins).Items;
            return results.Select(i => i.ContentLink);
        }

        public ITypeSearch<TContentData> FilterBySiteId(string siteId)
        {
            throw new NotImplementedException();
        }

        public ITypeSearch<TContentData> ChildrenOf(ContentReference parentLink)
        {
            throw new NotImplementedException();
        }

        public ITypeSearch<TContentData> IncludeWasteBasket()
        {
            throw new NotImplementedException();
        }

        public ITypeSearch<TContentData> OrderBy<TKey>(Expression<Func<TContentData, TKey>> keySelector)
        {
            throw new NotImplementedException();
        }

        public ITypeSearch<TContentData> OrderByDescending<TKey>(Expression<Func<TContentData, TKey>> keySelector)
        {
            throw new NotImplementedException();
        }

        public ITypeSearch<TContentData> Skip(int count)
        {
            throw new NotImplementedException();
        }

        public ITypeSearch<TContentData> Take(int count)
        {
            throw new NotImplementedException();
        }

        public ITypeSearch<TContentData> PublishedIgnoreDates()
        {
            throw new NotImplementedException();
        }

        public ITypeSearch<TContentData> Published()
        {
            var results = _searchClient.Search<TContentData>().Filter(p => p.IsPublished().Match(true));
            return results;
        }

        public ITypeSearch<TContentData> InRemoteSite(string remoteSite)
        {
            throw new NotImplementedException();
        }

        public ITypeSearch<TContentData> OfType(params Type[] type)
        {
            throw new NotImplementedException();
        }

        public int TakeCount { get; }
        public ITypeSearch<TContentData> FreeTextSearch(string query, IEnumerable<Expression<Func<TContentData, string>>> propertyExpressions, int minMatch = 0)
        {
            throw new NotImplementedException();
        }

        public ITypeSearch<TBlock> BlockSearch<TBlock>() where TBlock : BlockData
        {
            throw new NotImplementedException();
        }

        public ITypeSearch<TPage> PageSearch<TPage>() where TPage : PageData
        {
            throw new NotImplementedException();
        }

        public ITypeSearch<TEntry> NodeContentBaseSearch<TEntry>(Func<ITypeSearch<TEntry>, ITypeSearch<TEntry>> request) where TEntry : NodeContentBase
        {
            throw new NotImplementedException();
        }

        public ITypeSearch<TEntry> EntryContentBaseSearch<TEntry>(Func<ITypeSearch<TEntry>, ITypeSearch<TEntry>> request) where TEntry : EntryContentBase
        {
            throw new NotImplementedException();
        }

        public ITypeSearch<TEntry> VariantSearch<TEntry>(Func<ITypeSearch<TEntry>, ITypeSearch<TEntry>> request) where TEntry : VariationContent
        {
            throw new NotImplementedException();
        }

        public ITypeSearch<TEntry> ProductSearch<TEntry>(Func<ITypeSearch<TEntry>, ITypeSearch<TEntry>> request) where TEntry : ProductContent
        {
            throw new NotImplementedException();
        }

        public ITypeSearch<TEntry> GeneralSearch<TEntry>(Func<ITypeSearch<TEntry>, ITypeSearch<TEntry>> request) where TEntry : ContentData
        {
            throw new NotImplementedException();
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
