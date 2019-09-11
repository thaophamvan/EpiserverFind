using System;
using Core.Querying.Extensions;
using Core.Querying.Find.Extensions.Content;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Find;
using EPiServer.Find.Commerce;
using EPiServer.Find.Framework;

namespace Core.Querying.Find.Conventions
{
    public class SiteCatalogContentClientConventions : CatalogContentClientConventions
    {
        protected override void ApplyProductContentConventions(EPiServer.Find.ClientConventions.TypeConventionBuilder<ProductContent> conventionBuilder)
        {
            base.ApplyProductContentConventions(conventionBuilder);

            conventionBuilder
                .ExcludeField(x => x.Variations())
                .IncludeField(x => x.VariationContents());
            //.IncludeField(x => x.DefaultPrice())
            //.IncludeField(x => x.Prices());
        }

        public override void ApplyConventions(IClientConventions clientConventions)
        {
            try
            {
                base.ApplyConventions(clientConventions);
                // Uncomment line below if we don't index VariationContent
                // ContentIndexer.Instance.Conventions.ForInstancesOf<VariationContent>().ShouldIndex(x => false);
                SearchClient.Instance.Conventions.NestedConventions.ForInstancesOf<ProductContent>()
                    .Add(x => x.VariationContents());

            }
            catch (Exception ex)
            {
            }
        }
    }
}