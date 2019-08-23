using System.Collections.Generic;

namespace Core.Querying.Find.Models.Response
{
    public class SearchResponse<T>
    {
        public IPagedList<T> Items { get; set; }
        public List<FacetGroup> Facets { get; set; }
    }
}