using System.Configuration;

namespace Core.Querying.Infrastructure.Configuration
{
    internal class SiteSettingsProvider : ISiteSettingsProvider
    {
        public object GetSetting(string key)
        {
            return ConfigurationManager.GetSection(key);
        }

        public string GetSetting(string key, string defaultValue)
        {
            return ConfigurationManager.AppSettings[key] ?? defaultValue;
        }
    }
}