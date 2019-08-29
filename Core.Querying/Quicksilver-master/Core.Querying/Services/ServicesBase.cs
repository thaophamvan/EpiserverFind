using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Querying.Infrastructure;
using Electrolux.DataCache;
using EPiServer.ServiceLocation;

namespace Core.Querying.Services
{
    public class ServicesBase
    {
        private static readonly DataCacheHandler Cache = new DataCacheHandler();
        //private static readonly SiteSettingsHandler SiteSettings = ServiceLocator.Current.GetInstance<SiteSettingsHandler>();

        protected TResult ExecuteAndCache<TResult>(string cacheKey, List<string> cacheKeys, List<string> masterKeys, Func<TResult> methodToCall, TResult defaultResult, TimeSpan timeout, string resourceKey)
        {
            TResult result;
            if (Configuration.CacheEnabled)
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
            if (Configuration.CacheEnabled)
            {
                result = Cache.ExecuteAndCache(cacheKey, methodToCall, defaultResult, timeout, resourceKey);
                return result;
            }
            result = methodToCall.Invoke();

            return result == null ? defaultResult : result;
        }
    }
}
