using System;
using System.Collections.Generic;

namespace Electrolux.DataCache
{
    public interface IDataCacheStore
    {
        bool TryGetValue(string key, out object value);

        object Get(string key);

        void Insert(string key, object value, TimeSpan expiration, List<string> cacheKeys, List<string> masterKeys);

        void Remove(string key);

        bool AddIfNotExists(string key, object value, TimeSpan expiration, List<string> cacheKeys, List<string> masterKeys);

        bool AddIfNotExists(string key, object value, TimeSpan expiration, List<string> cacheKeys, List<string> masterKeys, out object existingValue);
    }
}
