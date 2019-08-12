using EPiServer.Core;
using EPiServer.Find;
using EPiServer.ServiceLocation;

namespace Core.Querying
{
    /// <summary>
    /// Provides methods to perform queries for pages and counting pages
    /// and caching the result.
    /// </summary>
    public class ContentDataQueryHandler : IContentDataQueryHandler
    {
        protected IContentDataQueryFactory PageQueryFactory { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentDataQueryHandler"/> class. 
        /// </summary>
        /// <param name="pageQueryFactory">Page query factory.</param>
        public ContentDataQueryHandler(IContentDataQueryFactory pageQueryFactory)
        {
            PageQueryFactory = pageQueryFactory;
        }

        #region Singleton implementation
        /// <summary>
        /// Gets a singleton instance of the class.
        /// </summary>
        /// <value>The singleton instance.</value>
        public static IContentDataQueryHandler Instance
        {
            get { return ServiceLocator.Current.GetInstance<IContentDataQueryHandler>(); }
        }
        #endregion

        /// <summary>
        /// <para>Produces a <see cref="ContentData" /> - query over pages of the given type.</para>
        /// <para>Search is performed in first and single CorePageProvider of current site.</para>
        /// </summary>

        public IClient Create()
        {
            return PageQueryFactory.Create(SearchEngineType.EpiserverFind).GetSearchEngine();
        }
    }
}
