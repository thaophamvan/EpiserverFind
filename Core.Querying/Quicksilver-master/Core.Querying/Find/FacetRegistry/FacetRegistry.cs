using System.Collections.Generic;
using System.Linq;
using Core.Querying.Find.Models.Request;

namespace Core.Querying.Find.FacetRegistry
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
                return _facets.Items.Select(x => x.Field);
            }
        }

        public bool IsTermFacetWithSpecificDataType(string fieldName, string dataType)
        {
            var facet = _facets.Items.FirstOrDefault(x => x.Field.ToLower() == fieldName.ToLower() && x.DataType == dataType);
            return facet != null;
        }

        public bool IsNumericRangeFacet(string fieldName)
        {
            var facet = _facets.Items.FirstOrDefault(x => x.Field.ToLower() == fieldName.ToLower());
            return facet != null && facet is NumericFacetItem;
        }
    }
}