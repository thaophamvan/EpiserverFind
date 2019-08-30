using System;
using System.Collections.Generic;
using Core.Querying.Infrastructure.Configuration;
using Electrolux.DataCache;

namespace Core.Querying.Services
{
    public class ServicesBase
    {
        private static readonly DataCacheHandler Cache = new DataCacheHandler();

        protected TResult ExecuteAndCache<TResult>(string cacheKey, List<string> cacheKeys, List<string> masterKeys, Func<TResult> methodToCall, TResult defaultResult, TimeSpan timeout, string resourceKey)
        {
            TResult result;
            if (SiteSettings.Instance.CacheEnabled)
            {
                result = Cache.ExecuteAndCache(cacheKey, cacheKeys, masterKeys, methodToCall, defaultResult, timeout, resourceKey);
                return result;
            }
            result = methodToCall.Invoke();

            return result == null ? defaultResult : result;
        }

        protected TResult ExecuteAndCache<TResult>(string cacheKey, Func<TResult> methodToCall, TResult defaultResult, TimeSpan timeout, string resourceKey)
        {
            TResult result;
            if (SiteSettings.Instance.CacheEnabled)
            {
                result = Cache.ExecuteAndCache(cacheKey, methodToCall, defaultResult, timeout, resourceKey);
                return result;
            }
            result = methodToCall.Invoke();

            return result == null ? defaultResult : result;
        }
    }
}
