using System;
using System.Drawing;
using System.Reflection;
using Core.Querying.ExpressionBuilder.Interfaces;
using Core.Querying.ExpressionBuilder.Models.Response;
using Core.Querying.Extensions;
using Core.Querying.Infrastructure.Configuration;
using Core.Querying.Infrastructure.ProtectedCall;
using Core.Querying.Shared;
using EPiServer.Core;
using EPiServer.Find;

namespace Core.Querying.Services
{
    public class MultipleSearchProvider : ServicesBase, IMultipleSearchProvider
    {
        private readonly ICircuitBreaker _circruiBreaker;
        private readonly Func<ServiceEnum, ISearchServices<IContent>> _searchServices;

        public MultipleSearchProvider(Func<ServiceEnum, ISearchServices<IContent>> searchServices, ICircuitBreaker circruiBreaker)
        {
            _searchServices = searchServices;
            _circruiBreaker = circruiBreaker;
        }

        public SearchResponse<TContent> GenericSearch<TContent>(ISearchRequest request) where TContent : IContent
        {
            var cacheKey = request.GetCacheKey();
            
            var result = ExecuteAndCache(
                GetType().Name + "." + MethodBase.GetCurrentMethod().Name + ":" + cacheKey,
                () =>
                {
                    var searchResult = _circruiBreaker.Execute<SearchResponse<TContent>>(
                        () => _searchServices(ServiceEnum.Find).GenericSearch(request) as SearchResponse<TContent>,
                        () => _searchServices(ServiceEnum.Cache).GenericSearch(request) as SearchResponse<TContent>
                        );
                    return searchResult;
                },
                EmptySearchResultsFactory.CreateSearchResponse<TContent>(),
                TimeSpan.FromMilliseconds(SiteSettings.Instance.ExecuteAndCacheTimeOutMilliseconds),
                "search");
            return result;
        }
    }
}
