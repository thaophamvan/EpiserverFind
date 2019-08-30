using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Querying.Extensions;
using Core.Querying.Find.Models.Request;
using Core.Querying.Find.Models.Response;
using Core.Querying.Infrastructure.ProtectedCall;
using Core.Querying.Shared;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.Find;

namespace Core.Querying.Services
{
    public class MultipleSearchProvider : ServicesBase, IMultipleSearchProvider
    {
        private readonly ICircuitBreaker _circruiBreaker = EPiServer.ServiceLocation.ServiceLocator.Current.GetInstance<ICircuitBreaker>();

        private readonly Func<string, ISearchServices> _searchServices;
        public MultipleSearchProvider(Func<string, ISearchServices> searchServices)
        {
            this._searchServices = searchServices;
        }
        public SearchResponse<TEntry> GenericSearch<TEntry>(ISearchRequest request) where TEntry : IContent
        {
            var cacheKey = request.GetCacheKey();

            var result = ExecuteAndCache(
                GetType().Name + "." + MethodBase.GetCurrentMethod().Name + ":" + cacheKey,
                () =>
                {
                    var searchResult = _circruiBreaker.Execute(
                        () => _searchServices(ServiceEnum.Find.ToString()).GenericSearch<TEntry>(request),
                        () => _searchServices(ServiceEnum.Cache.ToString()).GenericSearch<TEntry>(request)
                        );
                    return searchResult;
                },
                default(SearchResponse<TEntry>),
                TimeSpan.FromSeconds(30),
                "search");
            return result;
        }
    }
}
