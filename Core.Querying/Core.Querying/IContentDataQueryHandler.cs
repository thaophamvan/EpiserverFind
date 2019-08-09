using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using EPiServer.Core;
using EPiServer.Find;

namespace Core.Querying
{
    /// <summary>
    /// Provides methods to perform queries for pages and counting pages
    /// and caching the result.
    /// </summary>
    public interface IContentDataQueryHandler
    {
        /// <summary>
        /// <para>Produces a <see cref="PageQuery{T}" /> - query over pages of the given type.</para>
        /// <para>Search is performed in first and single CorePageProvider of current site.</para>
        /// </summary>
        /// <typeparam name="TPageData">A type of the pages to query.</typeparam>
        /// <returns>An <see cref="PageQuery{T}" /> value, allowing further page query manipulation.</returns>
        IClient Create();
    }
}
