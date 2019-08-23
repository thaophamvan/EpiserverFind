using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using Core.Querying.Find.Models.Request;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.Find;
using EPiServer.Find.Api.Querying;
using EPiServer.Find.Cms;
using EPiServer.Find.UnifiedSearch;
using EPiServer.Security;
using EPiServer.Shell.ContentQuery;

namespace Core.Querying
{
    public interface IContentDataQuery<TContentData> where TContentData : IContent
    {

        /// <summary>
        /// Executes the query returning only the ContentReference for each matching page.
        /// The query is executed without caching. Use with caution.
        /// </summary>
        /// <returns>ContentReference to the pages that match the query.</returns>
        IEnumerable<ContentReference> UnCachedContentReferencesResult(int cacheForSeconds = 60, bool cacheForEditorsAndAdmins = false);

        /// <summary>
        /// Executes the query returning only the ContentReference for each matching page.
        /// The result is either fetched from cache or added to cache
        /// </summary>
        /// <returns>Content references to the pages matching the query.</returns>
        IEnumerable<ContentReference> ContentReferencesResult(int cacheForSeconds = 60, bool cacheForEditorsAndAdmins = false);

        ITypeSearch<TContentData> FilterBySiteId(string siteId);

        /// <summary>
        /// Filters only pages with a 
        /// parent same as specified in a parameter <paramref name="parentLink"/>.
        /// </summary>
        /// <param name="parentLink">A parent link of result pages.</param>
        /// <returns>
        /// A ITypeSearch<TContentData/> whose elements include only pages 
        /// with Parent same as specified in <paramref name="parentLink"/>.
        /// </returns>
        ITypeSearch<TContentData> ChildrenOf(ContentReference parentLink);

        /// <summary>
        /// Includes pages from a wastebasket in a result view.
        /// </summary>
        /// <returns>
        /// A ITypeSearch<TContentData/> whose elements include pages 
        /// from a wastebasket.
        /// </returns>
        ITypeSearch<TContentData> IncludeWasteBasket();

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

        /// <summary>
        /// Find ContentData with specified types.
        /// </summary>
        /// <param name="types">List of types to find.</param>
        /// <returns>Returns query with applied filter by types.</returns>
        ITypeSearch<TContentData> OfType(params Type[] types);

        ITypeSearch<TContentData> FreeTextSearch(string query);

        ITypeSearch<TBlock> BlockSearch<TBlock>() where TBlock : BlockData;

        ITypeSearch<TPage> PageSearch<TPage>() where TPage : PageData;

        ITypeSearch<TEntry> NodeContentBaseSearch<TEntry>() where TEntry : NodeContentBase;

        ITypeSearch<TEntry> EntryContentBaseSearch<TEntry>() where TEntry : EntryContentBase;

        ITypeSearch<TEntry> VariantSearch<TEntry>()
            where TEntry : VariationContent;

        ITypeSearch<TEntry> ProductSearch<TEntry>()
            where TEntry : ProductContent;

        ITypeSearch<TEntry> GeneralSearch<TEntry>(ISearchRequest request) where TEntry : CatalogContentBase;

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