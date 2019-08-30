using System;
using System.Reflection;
using Core.Querying.Extensions;
using Core.Querying.Find.Models.Request;
using Core.Querying.Find.Models.Response;
using Core.Querying.Infrastructure.ProtectedCall;
using Core.Querying.Shared;
using EPiServer.Core;

namespace Core.Querying.Services
{
    public class MultipleSearchProvider : ServicesBase, IMultipleSearchProvider
    {
        private readonly ICircuitBreaker _circruiBreaker;
        private readonly Func<ServiceEnum, ISearchServices> _searchServices;

        public MultipleSearchProvider(Func<ServiceEnum, ISearchServices> searchServices, ICircuitBreaker circruiBreaker)
        {
            _searchServices = searchServices;
            _circruiBreaker = circruiBreaker;
        }
        public SearchResponse<TEntry> GenericSearch<TEntry>(ISearchRequest request) where TEntry : IContent
        {
            var cacheKey = request.GetCacheKey();

            var result = ExecuteAndCache(
                GetType().Name + "." + MethodBase.GetCurrentMethod().Name + ":" + cacheKey,
                () =>
                {
                    var searchResult = _circruiBreaker.Execute(
                        () => _searchServices(ServiceEnum.Find).GenericSearch<TEntry>(request),
                        () => _searchServices(ServiceEnum.Cache).GenericSearch<TEntry>(request)
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
