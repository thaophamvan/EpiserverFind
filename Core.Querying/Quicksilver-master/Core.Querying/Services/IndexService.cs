using System.Collections.Generic;
using System.Linq;
using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.Find.Framework;
using EPiServer.Find.Helpers;
using Mediachase.Commerce.Catalog;

namespace Core.Querying.Services
{
    public class IndexService : IIndexService
    {
        private readonly ReferenceConverter _referenceConverter;
        private readonly IContentRepository _contentRepository;
        
        public IndexService(ReferenceConverter referenceConverter, IContentRepository contentRepository)
        {
            _referenceConverter = referenceConverter;
            _contentRepository = contentRepository;
        }
        public void DoReIndexProducts(IEnumerable<string> catalogEntryCodes)
        {
            var productsToIndex = new List<ProductContent>();
            foreach (var variationCode in catalogEntryCodes)
            {
                var variationRef = _referenceConverter.GetContentLink(variationCode);
                if (variationRef != null)
                {
                    var variation = _contentRepository.Get<IContent>(variationRef) as VariationContent;
                    if (variation != null)
                    {
                        var productRefs = variation.GetParentProducts();
                        var products = _contentRepository.GetItems(productRefs, new LoaderOptions()).OfType<ProductContent>();
                        if (products != null && products.Any())
                        {
                            productsToIndex.AddRange(products);
                        }
                    }
                }
            }
            if (productsToIndex.Any())
            {
                SearchClient.Instance.Index(productsToIndex.DistinctBy(p=>p.Code));
            }
        }
    }
}