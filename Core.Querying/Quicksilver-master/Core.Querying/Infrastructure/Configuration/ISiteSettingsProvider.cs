namespace Core.Querying.Infrastructure.Configuration
{
    internal interface ISiteSettingsProvider
    {
        object GetSetting(string key);
        
        string GetSetting(string key, string defaultValue);                
    }
}