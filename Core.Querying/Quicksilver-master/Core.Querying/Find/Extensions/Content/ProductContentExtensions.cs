using System.Collections.Generic;
using System.Linq;
using EPiServer;
using EPiServer.Commerce.Catalog;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.Linking;
using EPiServer.Commerce.SpecializedProperties;
using EPiServer.ServiceLocation;

namespace Core.Querying.Find.Extensions.Content
{
    public static class ProductContentExtensions
    {
        #region Variantion
        public static IEnumerable<VariationContent> VariationContents(this ProductContent productContent)
        {
            return VariationContents(productContent, ServiceLocator.Current.GetInstance<IContentLoader>(), ServiceLocator.Current.GetInstance<IRelationRepository>());
        }

        public static IEnumerable<VariationContent> VariationContents(this ProductContent productContent, IContentLoader contentLoader, IRelationRepository relationRepository)
        {
            return contentLoader.GetItems(productContent.GetVariants(relationRepository), productContent.Language).OfType<VariationContent>();
        }
        #endregion

        #region Pricing
        public static Price DefaultPrice(this ProductContent productContent)
        {
            return DefaultPrice(productContent, ServiceLocator.Current.GetInstance<ReadOnlyPricingLoader>(), ServiceLocator.Current.GetInstance<IRelationRepository>());
        }

        public static Price DefaultPrice(this ProductContent productContent, ReadOnlyPricingLoader pricingLoader, IRelationRepository relationRepository)
        {
            var maxPrice = new Price();

            var variationLinks = productContent.GetVariants(relationRepository);
            foreach (var variationLink in variationLinks)
            {
                var defaultPrice = pricingLoader.GetDefaultPrice(variationLink);
                if (defaultPrice.UnitPrice.Amount > maxPrice.UnitPrice.Amount)
                {
                    maxPrice = defaultPrice;
                }
            }

            return maxPrice;
        }
        #endregion
    }
}