using System;

namespace Core.Querying
{
    public class ContentDataQueryFactory : IContentDataQueryFactory
    {
        public ISearchEngine Create(SearchEngineType searchEngineType)
        {
            switch (searchEngineType)
            {
                case SearchEngineType.EpiserverFind:
                    return new EpiserverFind();
                default:
                    throw new NotImplementedException("This bank type is unsupported");
            }
        }
    }
}
