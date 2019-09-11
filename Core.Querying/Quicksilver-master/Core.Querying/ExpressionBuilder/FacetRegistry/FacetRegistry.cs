using System.Collections.Generic;
using System.Linq;
using Core.Querying.ExpressionBuilder.Models.Request;

namespace Core.Querying.ExpressionBuilder.FacetRegistry
{
    public class FacetRegistry : IFacetRegistry
    {
        static FacetSpecification _facets = new FacetSpecification();
        public FacetSpecification Facets
        {
            get
            {
                return _facets;
            }
            set
            {
                _facets = value;
            }
        }

        public IEnumerable<string> FacetFieldNames
        {
            get
            {
                return _facets.Items.Select(x => x.PropertyId);
            }
        }
    }
}