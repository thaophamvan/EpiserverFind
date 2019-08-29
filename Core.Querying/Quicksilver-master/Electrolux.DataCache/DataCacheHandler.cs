using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using log4net;

namespace Electrolux.DataCache
{
    public sealed class DataCacheHandler : IDataCacheHandler
    {

        private readonly IDataCacheStore _dataCache;

        private readonly IDataCacheStore _dataTimeoutCache;

        private readonly IDataCacheStore _fetchFlagCache;

        private readonly IDataCacheStore _failCountCache;

        private static readonly ILog Log = LogManager.GetLogger(typeof(DataCacheHandler).Name);

        private int failCountLimit = 5;

        private TimeSpan execTimeout = new TimeSpan(0, 1, 0);

        private TimeSpan maxWaitForOtherThreadExec = new TimeSpan(0);

        private TimeSpan oldDataMaxTimeout = new TimeSpan(0);

        private TimeSpan manualDisableTimeout = new TimeSpan(1000000, 0, 0);

        private TimeSpan failCountTimeout = new TimeSpan(0, 10, 0);

        private static IDataCacheHandler instance;

        /// <summary>
        /// Gets or sets the maximum timout for how long old data will stay in the cache 
        /// after it has expired. If this value is lower than timeout in  the ExecuteAndCache 
        /// call then timeout will be used instead.
        /// </summary>
        /// <value>
        /// The old data max timeout value.
        /// </value>
        public TimeSpan OldDataMaxTimeout
        {
            get { return oldDataMaxTimeout; }
            set { oldDataMaxTimeout = value; }
        }

        /// <summary>
        /// Gets or sets the maximum time to wait for another threads Exec before
        /// returning default data. If set to zero no waiting is done.
        /// </summary>
        /// <value>
        /// The max wait for other thread exec.
        /// </value>
        public TimeSpan MaxWaitForOtherThreadExec
        {
            get { return maxWaitForOtherThreadExec; }
            set { maxWaitForOtherThreadExec = value; }
        }

        /// <summary>
        /// Gets or sets the number of failed execs before resource is disabled. Is automatically 
        /// reset after FailCountTimeout period without fails. 
        /// </summary>
        /// <value>
        /// Fail count limit.
        /// </value>
        public int FailCountLimit
        {
            get { return failCountLimit; }
            set { failCountLimit = value; }
        }

        /// <summary>
        /// Gets or sets the timeout, before a new execute attempt is made, for executing the 
        /// data fetch delegate in ExecuteAndCache.
        /// </summary>
        /// <value>
        /// Exec time out.
        /// </value>
        public TimeSpan ExecTimeOut
        {
            get { return execTimeout; }
            set { execTimeout = value; }
        }

        /// <summary>
        /// Gets or sets how long a resource is disabled after a call to Disable(). 
        /// </summary>
        /// <value>
        /// Manual disable timeout.
        /// </value>
        public TimeSpan ManualDisableTimeout
        {
            get { return manualDisableTimeout; }
            set { manualDisableTimeout = value; }
        }

        /// <summary>
        /// Gets or sets the timeout period without fails, before the fail count is reset to zero. 
        /// </summary>
        /// <value>
        /// Fail count tiemout.
        /// </value>
        public TimeSpan FailCountTimeout
        {
            get { return failCountTimeout; }
            set { failCountTimeout = value; }
        }

        /// <summary>
        /// Gets a singleton instance implementing IDataCacheHandler.
        /// </summary>
        /// <value>
        /// Singleton instance.
        /// </value>
        public static IDataCacheHandler Instance
        {
            get
            {
                Log.Info("DataCacheHandler instance initalized.");
                return instance ?? (instance = new DataCacheHandler("DataCacheInstance"));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataCacheHandler"/> class. 
        /// </summary>
        public DataCacheHandler()
            : this("DataCache")
        {            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataCacheHandler"/> class. Creating a new bucket of 
        /// cache items. This is accomplished by using another prefix for storing cached items.
        /// </summary>
        /// <param name="cachePrefix">
        /// The cache key prefix.
        /// </param>
        public DataCacheHandler(string cachePrefix)
        {
            _dataCache = new DataCacheStore(cachePrefix + ":", cachePrefix + ":");
            _dataTimeoutCache = new DataCacheStore(cachePrefix + "Timeout:", cachePrefix + ":");
            _fetchFlagCache = new DataCacheStore(cachePrefix + "FetchFlag:", cachePrefix + ":");
            _failCountCache = new DataCacheStore(cachePrefix + "FailCount:", cachePrefix + ":");
            GetAppSettings();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataCacheHandler"/> class. Enables
        /// possibility to change actual storage of cached items. Default is DataCacheStore.
        /// </summary>
        /// <param name="theDataCache">
        /// Storage of the actual data. It must be available 
        /// a little longer than the keys in DataTimeOutCache.
        /// </param>
        /// <param name="theDataTimeOutCache">
        /// One value stored for each DataCache value. These
        /// values has expire set to data expire time.
        /// </param>
        /// <param name="theFetchFlagCache">
        /// Cache for marking that fetching of data has been started by using the exec function.
        /// </param>
        /// <param name="theFailCountCache">
        /// Cache for counting the number of fails per resource.
        /// </param>
        public DataCacheHandler(IDataCacheStore theDataCache, IDataCacheStore theDataTimeOutCache, IDataCacheStore theFetchFlagCache, IDataCacheStore theFailCountCache)
        {
            _dataCache = theDataCache;
            _dataTimeoutCache = theDataTimeOutCache;
            _fetchFlagCache = theFetchFlagCache;
            _failCountCache = theFailCountCache;
            GetAppSettings();
        }

        public TResult Execute<TResult>(Func<TResult> methodToCall, TResult defaultResult, string resourceKey)
        {
            if (!this.ResourceIsDisabled(resourceKey))
            {
                try
                {
                    return methodToCall();
                }
                catch (Exception e)
                {
                    Log.Error("Failed to execute on resource: " + resourceKey, e);
                    this.IncFailCount(resourceKey);
                    if (this.ResourceIsDisabled(resourceKey))
                    {
                        Log.ErrorFormat("Resource {0} is auto disabled for {1} seconds.", resourceKey, this.failCountTimeout.TotalSeconds);
                    }
                }
            }
            return defaultResult;
        }

        /// <summary>
        /// Execute any function without parameters and return defined type of result.
        /// </summary>
        /// <typeparam name="TResult">Return type.</typeparam>
        /// <param name="key">The string key of the item.</param>
        /// <param name="methodToCall">The method that is called to fetch data.</param>
        /// <param name="defaultResult">If cache fetching and methodToCall fails to return data return this value.</param>
        /// <param name="timeout">The time to store data in the cache before a new fetch is done.</param>
        /// <param name="cacheKeys">The dependencies to other cached items, idetified by their keys.</param>
        /// <param name="masterKeys">The master keys that we depend upon. Master keys are used as markers to set up common dependencies without having to create the cache entries first.</param>
        /// <param name="resourceKey">Resource name used in Disable() call.</param>
        /// <returns>TResult type defined in the call.</returns>
        public TResult ExecuteAndCache<TResult>(string key,  List<string> cacheKeys, List<string> masterKeys, Func<TResult> methodToCall, TResult defaultResult, TimeSpan timeout, string resourceKey)
        {
            TResult cachedResult;
            bool manuallyDisabled;

            if (ResourceIsDisabled(resourceKey, out manuallyDisabled))
            {
                if (manuallyDisabled)
                {
                    return defaultResult;
                }
                return GetCachedOrDefault(key, _dataCache, defaultResult);
            }

            // Check for fast return, first check if value has timed out
            if (_dataTimeoutCache.Get(key) != null)
            {
                if (TryGetCached(key, _dataCache, out cachedResult))
                {
                    return cachedResult;
                }
            }

            // Cache has timed out. First mark that fetching is in progress
            if (_fetchFlagCache.AddIfNotExists(key, 1, execTimeout, cacheKeys, masterKeys))
            {
                try
                {
                    try
                    {
                        // Fetch new value
                        var newValue = methodToCall();
                        _dataCache.Insert(key, newValue, new TimeSpan(timeout.Ticks + Math.Max(timeout.Ticks, oldDataMaxTimeout.Ticks)), cacheKeys, masterKeys);
                        _dataTimeoutCache.Insert(key, 1, timeout, cacheKeys, masterKeys);
                        return newValue;
                    }
                    catch (Exception e)
                    {
                        Log.Error("Failed to execute on resource: " + resourceKey, e);
                        IncFailCount(resourceKey);
                        if (ResourceIsDisabled(resourceKey))
                        {
                            Log.ErrorFormat("Resource {0} is auto disabled for {1} seconds.", resourceKey, failCountTimeout.TotalSeconds);
                        }

                        // Remove timeout item because no new data was stored.
                        _dataTimeoutCache.Remove(key);

                        // Something went wrong return default
                        return defaultResult;
                    }
                }
                finally
                {
                    _fetchFlagCache.Remove(key);
                }
            }

            // The fetching is already in progress, return existing data
            if (TryGetCachedWithWait(key, _dataCache, out cachedResult))
            {
                return cachedResult;
            }
            return defaultResult;
        }

        public TResult ExecuteAndCache<TResult>(string cacheKey, Func<TResult> methodToCall, TResult defaultResult, TimeSpan timeout, string resourceKey)
        {
            return ExecuteAndCache(cacheKey, null, null, methodToCall, defaultResult, timeout, resourceKey);
        }

        public void Remove(string cacheKey)
        {
            _dataTimeoutCache.Remove(cacheKey);
            _dataCache.Remove(cacheKey);
            _fetchFlagCache.Remove(cacheKey);
        }

        public void RemoveMasterKey(string cacheKey)
        {
            _dataCache.Remove(cacheKey);
        }

        /// <summary>
        /// Disable all calls to execute on resource. Default data is always returned for 
        /// this resource when disabled.
        /// </summary>
        /// <param name="resourceKey">The unique key for the resource.</param>
        public void DisableResource(string resourceKey)
        {
            _failCountCache.Insert(resourceKey, failCountLimit * 2, manualDisableTimeout, null, null);
        }

        /// <summary>
        /// Enable all calls to execute on resource after it a Disable call.
        /// </summary>
        /// <param name="resourceKey">The unique key for the resource.</param>
        public void EnableResource(string resourceKey)
        {
            _failCountCache.Remove(resourceKey);
        }

        public int GetResourceFailCount(string resourceKey)
        {
            return GetCachedOrDefault(resourceKey, _failCountCache, 0);
        }

        public TResult GetCached<TResult>(string cacheKey)
        {
            var value = _dataCache.Get(cacheKey);

            if(value is TResult result)
            {
                return result;
            }
            else
            {
                return default(TResult);
            }
        }

        private TResult GetCachedOrDefault<TResult>(string cacheKey, TResult defaultResult)
        {
            return GetCachedOrDefault(cacheKey, _dataCache, defaultResult);
        }

        private bool TryGetCachedWithWait<TResult>(string cacheKey, IDataCacheStore cache, out TResult cachedResult)
        {
            int nextWaitMs = 20;
            int totalWaitMs = 0;

            // Use Sleep() instead of locks to avoid lock complexity.
            // This should not happen very often so performance is secondary.
            while (totalWaitMs < maxWaitForOtherThreadExec.TotalMilliseconds)
            {
                if (TryGetCached(cacheKey, cache, out cachedResult))
                {
                    return true;
                }

                // Wait a while and hope for executing thread to finish
                Thread.Sleep(nextWaitMs);
                totalWaitMs += nextWaitMs;
                nextWaitMs = Math.Min(2 * nextWaitMs, 200);
                if (totalWaitMs + nextWaitMs > maxWaitForOtherThreadExec.TotalMilliseconds)
                {
                    nextWaitMs = (int)maxWaitForOtherThreadExec.TotalMilliseconds - totalWaitMs;
                }
            }

            return TryGetCached(cacheKey, cache, out cachedResult);
        }

        private static bool TryGetCached<TResult>(string cacheKey, IDataCacheStore cache,  out TResult cachedResult)
        {
            TResult defaultDummy = default(TResult);
            var result = GetCachedOrDefault(cacheKey, cache, defaultDummy);
            if (!Equals(result, defaultDummy))
            {
                // Not default, so return from cache
                cachedResult = result;
                return true;
            }

            cachedResult = defaultDummy;
            return false;
        }

        private static TResult GetCachedOrDefault<TResult>(string cacheKey, IDataCacheStore cache, TResult defaultResult)
        {
            object item;
            if (cache.TryGetValue(cacheKey, out item))
            {
                if (item is TResult result)
                {
                    return result;
                }
            }

            return defaultResult;
        }

        private void IncFailCount(string resourceKey)
        {
            object countObj;
            if (!_failCountCache.AddIfNotExists(resourceKey, 1, failCountTimeout, null,null, out countObj))
            {
                if (countObj is int)
                {
                    _failCountCache.Insert(resourceKey, (int)countObj + 1, failCountTimeout, null, null);
                }
            }
        }

        private void GetAppSettings()
        {
            int value;
            if (int.TryParse(ConfigurationManager.AppSettings["DS_ManualDisableTimeoutMinutes"], out value))
            {
                ManualDisableTimeout = new TimeSpan(0, 0, value, 0);
            }
            if (int.TryParse(ConfigurationManager.AppSettings["DS_FailCountTimeoutSeconds"], out value))
            {
                FailCountTimeout = new TimeSpan(0, 0, 0, value);
            }
            if (int.TryParse(ConfigurationManager.AppSettings["DS_ExecTimeOutSeconds"], out value))
            {
                ExecTimeOut = new TimeSpan(0, 0, 0, value);
            }
            if (int.TryParse(ConfigurationManager.AppSettings["DS_MaxWaitForOtherThreadExecMilliseconds"], out value))
            {
                MaxWaitForOtherThreadExec = new TimeSpan(0, 0, 0, 0, value);
            }
            if (int.TryParse(ConfigurationManager.AppSettings["DS_OldDataMaxTimeoutSeconds"], out value))
            {
                OldDataMaxTimeout = new TimeSpan(0, 0, 0, value);
            }
            if (int.TryParse(ConfigurationManager.AppSettings["DS_FailCountLimit"], out value))
            {
                FailCountLimit = value;
            }
        }
        public bool ResourceIsDisabled(string resourceKey)
        {
            bool dummy;
            return ResourceIsDisabled(resourceKey, out dummy);
        }

        private bool ResourceIsDisabled(string resourceKey, out bool manuallyDisabled)
        {
            object countObj;
            if (_failCountCache.TryGetValue(resourceKey, out countObj))
            {
                if (countObj is int)
                {
                    manuallyDisabled = (int)countObj >= 2 * failCountLimit;
                    return (int)countObj >= failCountLimit;
                }
            }

            manuallyDisabled = false;
            return false;
        }

    }
}
