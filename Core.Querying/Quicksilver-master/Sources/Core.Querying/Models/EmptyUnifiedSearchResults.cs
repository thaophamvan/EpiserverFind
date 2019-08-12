using EPiServer.Find.UnifiedSearch;

namespace Core.Querying.Models
{
    public class EmptyUnifiedSearchResults : UnifiedSearchResults
    {
        public EmptyUnifiedSearchResults() : base(
            EmptySearchResultsFactory.CreateSearchResult<UnifiedSearchHit>()
        )
        { }
    }
}