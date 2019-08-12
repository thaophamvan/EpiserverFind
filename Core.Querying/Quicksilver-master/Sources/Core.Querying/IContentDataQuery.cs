using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.Find;
using EPiServer.Find.Api.Querying;
using EPiServer.Find.Cms;
using EPiServer.Find.UnifiedSearch;
using EPiServer.Security;

namespace Core.Querying
{
    public interface IContentDataQuery<TContentData>
    {
        /// <summary>
        /// Executes the query and returns the result using NoPageCollectionCaching
        /// as cache strategy, meaning that the result is never cached.
        /// Remember, with great power comes great responsibility.
        /// </summary>
        /// <param name="asAnyVersion">
        /// Get pages with "IsAnyVersion" flag.
        /// </param>
        /// <returns>
        /// The result of the query.
        /// </returns>
        IEnumerable<TContentData> UnCachedResult(bool asAnyVersion = false);

        IContentResult<TContentData> UnCachedResult<TContentData>(ITypeSearch<TContentData> search, int cacheForSeconds = 60, bool cacheForEditorsAndAdmins = false) where TContentData : IContentData;

        /// <summary>
        /// Executes the query and returns the result using NoPageCollectionCaching
        /// as cache strategy, meaning that the result is never cached.
        /// Remember, with great power comes great responsibility.
        /// </summary>
        /// <param name="pageProviderKey">
        /// The page Provider Key.
        /// </param>
        /// <returns>
        /// The result of the query.
        /// </returns>
        IEnumerable<TContentData> UnCachedResult(string pageProviderKey);

        /// <summary>
        /// Returns the result of the query using the IPageCollectionCacheStrategy
        /// specified by previous calls to the CacheStrategy method, or the default
        /// cache strategy if no calls to the CacheStrategy method has been made.
        /// </summary>
        /// <param name="pageProviderKey">
        /// The page Provider Key.
        /// </param>
        /// <param name="cacheKey">
        /// The cache key used for caching the query result.
        /// </param>
        /// <returns>
        /// The result of the query, either from cache or from the database.
        /// </returns>
        IEnumerable<TContentData> Result(string pageProviderKey, string cacheKey);

        /// <summary>
        /// Returns the result of the query using the IPageCollectionCacheStrategy
        /// specified by previous calls to the CacheStrategy method, or the default
        /// cache strategy if no calls to the CacheStrategy method has been made.
        /// </summary>
        /// <param name="cacheKey">
        /// The cache key used for caching the query result.
        /// </param>
        /// <returns>
        /// The result of the query, either from cache or from the database.
        /// </returns>
        IEnumerable<TContentData> Result(string cacheKey);

        /// <summary>
        /// Returns collection of page data corresponded to given page references or its cached value.
        /// </summary>
        /// <param name="cacheKey">Cache key.</param>
        /// <param name="getReferences">Function that returns references to pages.</param>
        /// <returns>Collection of read page data or its cached value.</returns>
        IEnumerable<TContentData> Result(string cacheKey, Func<ReadOnlyCollection<PageReference>> getReferences);

        /// <summary>
        /// Executes the query returning only the PageReference for each matching page.
        /// The query is executed without caching. Use with caution.
        /// </summary>
        /// <returns>PageReferences to the pages that match the query.</returns>
        IEnumerable<PageReference> UnCachedPageReferencesResult();

        /// <summary>
        /// Executes the query returning only the PageReference for each matching page.
        /// The result is either fetched from cache or added to cache using the 
        /// IPageCollectionCacheStrategy specified by previous calls to the CacheStrategy 
        /// method, or the default cache strategy if no calls to the CacheStrategy method 
        /// has been made.
        /// </summary>
        /// <param name="cacheKey">The cache key used for caching the query result.</param>
        /// <returns>Page references to the pages matching the query.</returns>
        IEnumerable<PageReference> PageReferencesResult(string cacheKey);

        ITypeSearch<TSource> Where<TSource>(ITypeSearch<TSource> search, bool condition,
            Expression<Func<TSource, Filter>> filterExpression);

        ITypeSearch<TSource> Where<TSource>(ITypeSearch<TSource> search, Func<ITypeSearch<TSource>, ITypeSearch<TSource>> request);

        ITypeSearch<TContentData> FilterBySiteId(string siteId);

        /// <summary>
        /// Filters original PageQuery&lt;TPageData&gt; to include only pages with a 
        /// parent same as specified in a parameter <paramref name="parentLink"/>.
        /// </summary>
        /// <param name="parentLink">A parent link of result pages.</param>
        /// <returns>
        /// A PageQuery&lt;TPageData&gt; whose elements include only pages 
        /// with Parent same as specified in <paramref name="parentLink"/>.
        /// </returns>
        ITypeSearch<TContentData> ChildrenOf(PageReference parentLink);

        /// <summary>
        /// Includes pages from a wastebasket in a result view.
        /// </summary>
        /// <returns>
        /// A PageQuery&lt;TPageData&gt; whose elements include pages 
        /// from a wastebasket.
        /// </returns>
        ITypeSearch<TContentData> IncludeWasteBasket();

        /// <summary>
        /// Sorts the pages in a sequence in ascending order according to a key.
        /// </summary>
        /// <typeparam name="TKey">
        /// The type of the key returned by the function that is represented 
        /// by keySelector.
        /// </typeparam>
        /// <param name="keySelector">A function to extract a key from a page.</param>
        /// <returns>
        /// A PageQuery&lt;TPageData&gt; whose elements are sorted according to a key.
        /// </returns>
        ITypeSearch<TContentData> OrderBy<TKey>(Expression<Func<TContentData, TKey>> keySelector);

        /// <summary>
        /// Sorts the pages in a sequence in descending order according to a key.
        /// </summary>
        /// <typeparam name="TKey">
        /// The type of the key returned by the function that is represented 
        /// by keySelector.
        /// </typeparam>
        /// <param name="keySelector">A function to extract a key from a page.</param>
        /// <returns>
        /// A PageQuery&lt;TPageData&gt; whose elements are sorted 
        /// in descending order according to a key.
        /// </returns>
        ITypeSearch<TContentData> OrderByDescending<TKey>(Expression<Func<TContentData, TKey>> keySelector);

        /// <summary>
        /// Bypasses a specified number of pages in a sequence and then returns 
        /// the remaining pages.
        /// </summary>
        /// <param name="count">
        /// The number of pages to skip before returning the remaining pages.
        /// </param>
        /// <returns>
        /// A PageQuery&lt;TPageData&gt; that contains the pages that occur 
        /// after the specified index in the input sequence.
        /// </returns>
        ITypeSearch<TContentData> Skip(int count);

        /// <summary>
        /// Returns a specified number of contiguous pages from the start of a pages sequence.
        /// </summary>
        /// <param name="count">The number of elements to return.</param>
        /// <returns>
        /// A PageQuery&lt;TPageData&gt; that contains the specified number of 
        /// elements from the start of the input pages sequence.</returns>
        ITypeSearch<TContentData> Take(int count);

        /// <summary>
        /// Applies additional filtering: only published pages ignoring <see cref="PageData.StartPublish"/> and <see cref="PageData.StopPublish"/> dates.
        /// </summary>
        /// <returns>Filtered query.</returns>
        ITypeSearch<TContentData> PublishedIgnoreDates();

        /// <summary>
        /// Applies additional filtering: only published pages.
        /// </summary>
        /// <returns>Filtered query.</returns>
        ITypeSearch<TContentData> Published();

        /// <summary>
        /// Applies additional filtering: only pages in specified remote site.
        /// </summary>
        /// <param name="remoteSite">
        /// The remote Site.
        /// </param>
        /// <returns>
        /// Filtered query.
        /// </returns>
        ITypeSearch<TContentData> InRemoteSite(string remoteSite);

        ITypeSearch<TContentData> OfType(params Type[] type);

        int TakeCount { get; }

        ITypeSearch<TContentData> FreeTextSearch(string query, IEnumerable<Expression<Func<TContentData, string>>> propertyExpressions, int minMatch = 0);

        ITypeSearch<TBlock> BlockSearch<TBlock>() where TBlock : BlockData;
        ITypeSearch<TPage> PageSearch<TPage>() where TPage : PageData;
        ITypeSearch<TEntry> NodeContentBaseSearch<TEntry>(Func<ITypeSearch<TEntry>, ITypeSearch<TEntry>> request) where TEntry : NodeContentBase;
        ITypeSearch<TEntry> EntryContentBaseSearch<TEntry>(Func<ITypeSearch<TEntry>, ITypeSearch<TEntry>> request) where TEntry : EntryContentBase;

        ITypeSearch<TEntry> VariantSearch<TEntry>(Func<ITypeSearch<TEntry>, ITypeSearch<TEntry>> request)
            where TEntry : VariationContent;
        ITypeSearch<TEntry> ProductSearch<TEntry>(Func<ITypeSearch<TEntry>, ITypeSearch<TEntry>> request)
            where TEntry : ProductContent;

        ITypeSearch<TEntry> GeneralSearch<TEntry>(Func<ITypeSearch<TEntry>, ITypeSearch<TEntry>> request)
            where TEntry : ContentData;

        ITypeSearch<TEntry> MultiSearch<TEntry>(Func<ITypeSearch<TEntry>, ITypeSearch<TEntry>> request)
            where TEntry : ContentData;
        ITypeSearch<TEntry> MultiUnifiedSearch<TEntry>(Func<ITypeSearch<TEntry>, ITypeSearch<TEntry>> request)
            where TEntry : ContentData;
        ITypeSearch<TEntry> DynamicMultiSearch<TEntry>(Func<ITypeSearch<TEntry>, ITypeSearch<TEntry>> request)
            where TEntry : ContentData;
        


        ITypeSearch<ISearchContent> GeneralUnifiedSearch<TEntry>(Func<ITypeSearch<TEntry>, ITypeSearch<TEntry>> request)
            where TEntry : ContentData;
    }
}