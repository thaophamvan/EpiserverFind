using System.Collections.Generic;
using Core.Querying.Find.Models.Request;

namespace Core.Querying.Find.FacetRegistry
{
    public interface IFacetRegistry
    {
        FacetSpecification Facets { get; set; }
        IEnumerable<string> FacetFieldNames { get; }
        bool IsNumericRangeFacet(string fieldName);
        bool IsTermFacetWithSpecificDataType(string fieldName, string dataType);
    }
}
