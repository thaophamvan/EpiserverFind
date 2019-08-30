using System;

namespace Core.Querying.Infrastructure.Configuration
{    
    public class SiteSettings
    {
        private static readonly Lazy<SiteSettings> SiteSettingsInstance = new Lazy<SiteSettings>(() => new SiteSettings());
        private readonly ISiteSettingsProvider _siteSettingsProvider = new SiteSettingsProvider();        

        private SiteSettings()
        {
        }

        public static SiteSettings Instance
        {
            get { return SiteSettingsInstance.Value; }
        }

      
        public int ExecuteAndCacheTimeOutMilliseconds
        {
            get { return GetSetting("ExecuteAndCacheTimeOut", 30); }
        }

        public int FindCacheTimeOutMinutes
        {
            get { return GetSetting("FindCacheTimeOut", 5); }
        }

        public bool CacheEnabled
        {
            get
            {
                try
                {
                    var setting = GetSetting("CacheEnabled", "true").ToLower();
                    return bool.Parse(setting);
                }
                catch
                {
                    return false;
                }
            }
        }

        private TimeSpan GetSetting(string key, TimeSpan defaultValue)
        {
            TimeSpan result;
            var setting = GetSetting(key, string.Empty);

            return TimeSpan.TryParse(setting, out result) ? result : defaultValue;
        }

        private string GetSetting(string key, string defaultValue)
        {
            return _siteSettingsProvider.GetSetting(key, defaultValue);
        }

        private int GetSetting(string key, int defaultValue)
        {
            int result;
            var setting = GetSetting(key, string.Empty);

            return int.TryParse(setting, out result) ? result : defaultValue;
        }

        private bool GetSetting(string key, bool defaultValue)
        {
            bool result;
            var setting = GetSetting(key, string.Empty);

            return bool.TryParse(setting, out result) ? result : defaultValue;
        }

        private object GetSetting(string key)
        {
            return _siteSettingsProvider.GetSetting(key);
        }
    }
}
