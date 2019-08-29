using System.Configuration;
using Core.Querying.Find.Models.Request;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Find;

namespace Core.Querying.Services
{
    public interface IMultipleSearchProvider
    {
        void Initialize(ProviderSettings providerSettings);
    }
}
