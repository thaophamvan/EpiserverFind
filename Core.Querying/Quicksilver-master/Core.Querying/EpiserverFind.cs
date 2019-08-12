using EPiServer.Core;
using EPiServer.Find;
using EPiServer.Find.Framework;
using EPiServer.ServiceLocation;

namespace Core.Querying
{
    public class EpiserverFind : ISearchEngine
    {
        public IClient GetSearchEngine()
        {
            return SearchClient.Instance;
        }
    }
}
