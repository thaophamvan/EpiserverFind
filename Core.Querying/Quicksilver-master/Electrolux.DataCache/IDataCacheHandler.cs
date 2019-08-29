using System;
using System.Collections.Generic;

namespace Electrolux.DataCache
{
    public interface IDataCacheHandler
    {
        TResult Execute<TResult>(Func<TResult> methodToCall, TResult defaultResult, string resourceKey);

        TResult ExecuteAndCache<TResult>(string cacheKey, Func<TResult> methodToCall, TResult defaultResult, TimeSpan timeout, string resourceKey);

        TResult ExecuteAndCache<TResult>(string cacheKey, List<string> cacheKeys, List<string> masterKeys, Func<TResult> methodToCall, TResult defaultResult, TimeSpan timeout, string resourceKey);

        TResult GetCached<TResult>(string cacheKey);

        void Remove(string cacheKey);
        void RemoveMasterKey(string cacheKey);

        int FailCountLimit { get; set; }

        TimeSpan ExecTimeOut { get; set; }

        TimeSpan ManualDisableTimeout { get; set; }

        TimeSpan FailCountTimeout { get; set; }

        TimeSpan OldDataMaxTimeout { get; set; }

        TimeSpan MaxWaitForOtherThreadExec { get; set; }

        void DisableResource(string resourceKey);

        void EnableResource(string resourceKey);

        int GetResourceFailCount(string resourceKey);

        bool ResourceIsDisabled(string resourceKey);

    }
}
