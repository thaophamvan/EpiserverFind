using System;
using System.Collections.Generic;
using Core.Querying.Find.Models.Request;
using Core.Querying.Find.Models.Response;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.Find;

namespace Core.Querying.Services
{
    public interface ISearchServices<TContent> where TContent : IContent
    {
        SearchResponse<TContent> GenericSearch(ISearchRequest request);
        
        /// <summary>
        /// Executes the query returning only the ContentReference for each matching page.
        /// The result is either fetched from cache or added to cache
        /// </summary>
        /// <returns>Content references to the pages matching the query.</returns>
        IEnumerable<ContentReference> ContentReferencesResult(int cacheForSeconds = 60, bool cacheForEditorsAndAdmins = false);

        SearchResponse<TContent> FilterBySiteId(string siteId);

        /// <summary>
        /// Filters only pages with a 
        /// parent same as specified in a parameter <paramref name="parentLink"/>.
        /// </summary>
        /// <param name="parentLink">A parent link of result pages.</param>
        /// <returns>SearchResponse</returns>
        SearchResponse<TContent> ChildrenOf(ContentReference parentLink);

        /// <summary>
        /// Includes pages from a wastebasket in a result view.
        /// </summary>
        /// <returns>SearchResponse</returns>
        SearchResponse<TContent> IncludeWasteBasket();

        /// <summary>
        /// Applies additional filtering: only published pages ignoring <see cref="PageData.StartPublish"/> and <see cref="PageData.StopPublish"/> dates.
        /// </summary>
        /// <returns>SearchResponse.</returns>
        SearchResponse<TContent> PublishedIgnoreDates();

        /// <summary>
        /// Applies additional filtering: only published pages.
        /// </summary>
        /// <returns>SearchResponse.</returns>
        SearchResponse<TContent> Published();

        /// <summary>
        /// Find ContentData with specified types.
        /// </summary>
        /// <param name="types">List of types to find.</param>
        /// <returns>SearchResponse.</returns>
        SearchResponse<TContent> OfType(params Type[] types);

        SearchResponse<TContent> FreeTextSearch(string query, int maxItemNumber);

        SearchResponse<TContent> MultiSearch(Func<ITypeSearch<TContent>, ITypeSearch<TContent>> request);

        SearchResponse<TContent> MultiUnifiedSearch(Func<ITypeSearch<TContent>, ITypeSearch<TContent>> request);

        SearchResponse<TContent> DynamicMultiSearch(Func<ITypeSearch<TContent>, ITypeSearch<TContent>> request);

        SearchResponse<TContent> GenericUnifiedSearch(Func<ITypeSearch<TContent>, ITypeSearch<TContent>> request);
    }
}
