using System.Collections.Generic;

namespace Core.Querying.ExpressionBuilder.Models.Response
{
    public class FacetGroup
    {
        public string GroupName { get; set; }

        public List<FacetFilter> Items { get; set; }
    }
}