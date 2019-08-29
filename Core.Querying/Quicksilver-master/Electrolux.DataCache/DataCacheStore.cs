using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Framework.Cache;
using EPiServer.ServiceLocation;

namespace Electrolux.DataCache
{
    internal sealed class DataCacheStore : IDataCacheStore
    {
        private readonly ISynchronizedObjectInstanceCache _cacheManager = ServiceLocator.Current.GetInstance<ISynchronizedObjectInstanceCache>();

        private readonly string _cacheKeyPrefix = string.Empty;
        private readonly string _keyPrefixDependency = string.Empty;

        public DataCacheStore(string keyPrefix, string keyPrefixDependency)
        {
            _cacheKeyPrefix = keyPrefix;
            _keyPrefixDependency = keyPrefixDependency;
        }

        public bool TryGetValue(string key, out object value)
        {
            var internalKey = _cacheKeyPrefix + key;
            value = _cacheManager.Get(internalKey);
            return value != null;
        }

        public object Get(string key)
        {
            var internalKey = _cacheKeyPrefix + key;
            return _cacheManager.Get(internalKey);
        }

        /// <summary>
        /// <summary>Inserts an item in the cache.</summary>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiration"></param>
        /// <param name="cacheKeys">The dependencies to other cached items, idetified by their keys.</param>
        /// <param name="masterKeys">The master keys that we depend upon. Master keys are used as markers to set up common dependencies without having to create the cache entries first.
        ///If you set up a master key dependency, there is no need for the corresponding entry to exist before adding
        ///something that depends on the master key.
        /// The concept of master keys could be regarded as similar to the cache region concept, but using master keys
        ///allows you to have more than one, where cache regions is restricted to one per cached item - you can only
        ///place the item in one region.
        /// </param>

        public void Insert(string key, object value, TimeSpan expiration, List<string> cacheKeys, List<string> masterKeys)
        {
            if(value == null)
            {
                return;
            }

            if (masterKeys == null)
            {
                masterKeys = new List<string>();
            }

            if (cacheKeys == null)
            {
                cacheKeys = new List<string>();
            }

            CacheEvictionPolicy cacheEvictionPolicy;
            if (!cacheKeys.Any() && !masterKeys.Any())
            {
                cacheEvictionPolicy = new CacheEvictionPolicy(expiration, CacheTimeoutType.Absolute);
            }
            else if(masterKeys.Any() && !cacheKeys.Any())
            {
                cacheEvictionPolicy = new CacheEvictionPolicy(expiration, CacheTimeoutType.Absolute, null, masterKeys.Select(k=>
                    $"{_keyPrefixDependency}{k}"));
            }
            else if (!masterKeys.Any() && cacheKeys.Any())
            {
                cacheEvictionPolicy = new CacheEvictionPolicy(expiration, CacheTimeoutType.Absolute, cacheKeys.Select(k =>
                    $"{_keyPrefixDependency}{k}"));
            }
            else if (masterKeys.Any() && cacheKeys.Any())
            {
                cacheEvictionPolicy = new CacheEvictionPolicy(expiration, CacheTimeoutType.Absolute, cacheKeys.Select(k =>
                    $"{_keyPrefixDependency}{k}"), masterKeys.Select(k =>
                    $"{_keyPrefixDependency}{k}"));
            }
            else
            {
                cacheEvictionPolicy = new CacheEvictionPolicy(expiration, CacheTimeoutType.Absolute);
            }

            var internalKey = _cacheKeyPrefix + key;
            _cacheManager.Insert(internalKey, value, cacheEvictionPolicy);
        }

        
        public void Remove(string key)
        {
            var internalKey = _cacheKeyPrefix + key;
            _cacheManager.Remove(internalKey);
        }

        public bool AddIfNotExists(string key, object value, TimeSpan expiration, List<string> cacheKeys, List<string> masterKeys, out object existingValue)
        {
            var internalKey = _cacheKeyPrefix + key;
            var oldValue = Get(internalKey);
            if (oldValue == null)
            {
                Insert(key, value, expiration, cacheKeys, masterKeys);
            }

            existingValue = oldValue;
            return oldValue == null;
        }

        public bool AddIfNotExists(string key, object value, TimeSpan expiration, List<string> cacheKeys, List<string> masterKeys)
        {
            object dummy;
            return AddIfNotExists(key, value, expiration, cacheKeys, masterKeys, out dummy);
        }

    }
}
