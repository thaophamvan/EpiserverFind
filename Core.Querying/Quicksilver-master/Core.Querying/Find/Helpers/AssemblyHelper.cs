using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EPiServer.Commerce.Catalog.ContentTypes;

namespace Core.Querying.Find.Helpers
{
    public class AssemblyHelper
    {
        private const string PRODUCT_PREFIX = "Product";
        private static IEnumerable<Type> productTypes;
        public static IEnumerable<Type> ProductTypes
        {
            get
            {
                if (productTypes == null || !productTypes.Any())
                {
                    var assembly = Assembly.GetAssembly(typeof(ProductContent));
                    if (assembly != null)
                    {
                        productTypes = assembly.GetTypes().Where(x => x.Name.EndsWith(PRODUCT_PREFIX));
                    }
                }
                return productTypes;
            }
        }

        public static Type GetProductTypeWithProperty(string propertyName)
        {
            if (ProductTypes == null || !ProductTypes.Any())
            {
                return null;
            }
            return productTypes.FirstOrDefault(x => x.GetProperty(propertyName) != null);
        }
    }
}