using EPiServer.Core;

namespace Core.Querying
{
    public interface IContentDataQueryFactory
    {
        ISearchEngine Create(SearchEngineType searchEngineType);
    }
}
