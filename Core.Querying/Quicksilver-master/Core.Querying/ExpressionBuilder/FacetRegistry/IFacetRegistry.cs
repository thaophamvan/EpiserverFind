using System.Collections.Generic;
using Core.Querying.ExpressionBuilder.Models.Request;

namespace Core.Querying.ExpressionBuilder.FacetRegistry
{
    public interface IFacetRegistry
    {
        FacetSpecification Facets { get; set; }
        IEnumerable<string> FacetFieldNames { get; }
    }
}
