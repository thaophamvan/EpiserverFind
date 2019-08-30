using System;
using System.Reflection;
using Core.Querying.Extensions;
using Core.Querying.Find.Models.Request;
using Core.Querying.Find.Models.Response;
using Core.Querying.Infrastructure.Configuration;
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

        public SearchResponse<T> GenericSearch<T>(ISearchRequest request) where T : IContent
        {
            var cacheKey = request.GetCacheKey();

            var result = ExecuteAndCache(
                GetType().Name + "." + MethodBase.GetCurrentMethod().Name + ":" + cacheKey,
                () =>
                {
                    var searchResult = _circruiBreaker.Execute(
                        () => _searchServices(ServiceEnum.Find).GenericSearch<T>(request),
                        () => _searchServices(ServiceEnum.Cache).GenericSearch<T>(request)
                        );
                    return searchResult;
                },
                EmptySearchResultsFactory.CreateSearchResponse<T>(),
                TimeSpan.FromMilliseconds(SiteSettings.Instance.ExecuteAndCacheTimeOutMilliseconds),
                "search");
            return result;
        }
    }
}
