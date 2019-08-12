using EPiServer.Core;
using EPiServer.Find;
using EPiServer.Find.Framework;
using EPiServer.ServiceLocation;

namespace Core.Querying
{
    public interface ISearchEngine
    {
        IClient GetSearchEngine();
    }
}
