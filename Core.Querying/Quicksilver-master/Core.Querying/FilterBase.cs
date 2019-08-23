using EPiServer.Core;
using EPiServer.Find;
using EPiServer.Find.Cms;

namespace Core.Querying
{
    public class FilterBase
    {
        protected readonly IClient FindClient = ContentDataQueryHandler.Instance.Create();
        public ITypeSearch<T> ApplyFilterBaseContent<T>() where T : IContent
        {
            var searchResult = FindClient.Search<T>();
            return searchResult.FilterForVisitor();
        }
        public ITypeSearch<T> ApplyFilterBaseContentData<T>() where T : IContentData
        {
            var searchResult = FindClient.Search<T>();
            return searchResult.FilterForVisitor();
        }
    }
}
