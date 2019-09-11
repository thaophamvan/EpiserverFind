using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EPiServer.Commerce.Catalog.ContentTypes;

namespace Core.Querying.ExpressionBuilder.Helpers
{
    public class AssemblyHelper
    {
        private const string ProductPrefix = "Product";
        private static IEnumerable<Type> _productTypes;
        public static IEnumerable<Type> ProductTypes
        {
            get
            {
                if (_productTypes == null || !_productTypes.Any())
                {
                    var assembly = Assembly.GetAssembly(typeof(ProductContent));
                    if (assembly != null)
                    {
                        _productTypes = assembly.GetTypes().Where(x => x.Name.EndsWith(ProductPrefix));
                    }
                }
                return _productTypes;
            }
        }

        public static Type GetProductTypeWithProperty(string propertyName)
        {
            if (ProductTypes == null || !ProductTypes.Any())
            {
                return null;
            }
            return _productTypes.FirstOrDefault(x => x.GetProperty(propertyName) != null);
        }
    }
}