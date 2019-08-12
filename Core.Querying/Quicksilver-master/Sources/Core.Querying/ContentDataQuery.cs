using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using EPiCode.DynamicMultiSearch;
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
    /// Provides functionality to evaluate queries against a CorePageProvider.
    /// </summary>
    /// <typeparam name="TContentData">
    /// The type of the pages to query. This type parameter is covariant. 
    /// That is, you can use either the type you specified or any type 
    /// that is more derived.
    /// </typeparam>
    public class ContentDataQuery<TContentData> : IContentDataQuery<TContentData>
    {
        public IEnumerable<TContentData> UnCachedResult(bool asAnyVersion = false)
        {
            throw new NotImplementedException();
        }

        public IContentResult<TContentData> UnCachedResult<TContentData>(ITypeSearch<TContentData> search, int cacheForSeconds = 60,
            bool cacheForEditorsAndAdmins = false) where TContentData : IContentData
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TContentData> UnCachedResult(string pageProviderKey)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TContentData> Result(string pageProviderKey, string cacheKey)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TContentData> Result(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TContentData> Result(string cacheKey, Func<ReadOnlyCollection<PageReference>> getReferences)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PageReference> UnCachedPageReferencesResult()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PageReference> PageReferencesResult(string cacheKey)
        {
            throw new NotImplementedException();
        }

        public ITypeSearch<TSource> Where<TSource>(ITypeSearch<TSource> search, bool condition, Expression<Func<TSource, Filter>> filterExpression)
        {
            return search.Filter<TSource>(filterExpression);
        }

        public ITypeSearch<TSource> Where<TSource>(ITypeSearch<TSource> search, Func<ITypeSearch<TSource>, ITypeSearch<TSource>> request)
        {
            return request(search);
        }

        public ITypeSearch<TContentData> FilterBySiteId(string siteId)
        {
            throw new NotImplementedException();
        }

        public ITypeSearch<TContentData> ChildrenOf(PageReference parentLink)
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
            throw new NotImplementedException();
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

        public ITypeSearch<TEntry> NodeContentBaseSearch<TEntry>(Func<ITypeSearch<TEntry>, ITypeSearch<TEntry>> request) where TEntry : NodeContentBase
        {
            throw new NotImplementedException();
        }
    }
}
