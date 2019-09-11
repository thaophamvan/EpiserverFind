using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Core.Querying.ExpressionBuilder.Interfaces;
using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.Events;
using EPiServer.Events.Clients;
using EPiServer.Find.Api;
using EPiServer.Find.Cms;
using EPiServer.Find.Cms.Conventions;
using EPiServer.Find.Framework;
using EPiServer.Find.Helpers;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Engine.Events;
using InitializationModule = EPiServer.Web.InitializationModule;

namespace Core.Querying.Infrastructure.Initialization
{
    [ModuleDependency(typeof(InitializationModule))]
    public class EPiServerFindInitialization : IInitializableModule
    {
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <remarks>
        /// Gets called as part of the EPiServer Framework initialization sequence. Note that it will be called
        /// only once per AppDomain, unless the method throws an exception. If an exception is thrown, the initialization
        /// method will be called repeadetly for each request reaching the site until the method succeeds.
        /// </remarks>
        public void Initialize(InitializationEngine context)
        {
            // Exclude all from indexing
            //ContentIndexer.Instance.Conventions.ForInstancesOf<IContent>().ShouldIndex(x => false);

            // Include all pages
            ContentIndexer.Instance.Conventions.ForInstancesOf<ICanBeSearched>().ShouldIndex(ShouldIndexSitePageData);

            //Include blocks
            ContentIndexer.Instance.Conventions.ForInstancesOf<BlockData>().ShouldIndex(m => true);

            //Include images
            //ContentIndexer.Instance.Conventions.ForInstancesOf<ImageFile>().ShouldIndex(m => true);

            // Include all catalog items
            ContentIndexer.Instance.Conventions.ForInstancesOf<CatalogContentBase>().ShouldIndex(ShouldIndexCatalogData);

            
            //try
            //{
            //    SearchClient.Instance.Conventions.NestedConventions.ForInstancesOf<Produc>().Add(p => p.IsProductAvailability);
            //    SearchClient.Instance.Conventions.NestedConventions.ForInstancesOf<BaseProduct>().Add(p => p.Prices);
            //    SearchClient.Instance.Conventions.NestedConventions.ForInstancesOf<BaseVariation>().Add(p => p.Prices);
            //    SearchClient.Instance.Conventions.NestedConventions.ForInstancesOf<BaseProduct>().Add(p => p.IsProductClearance);
            //}
            //catch (ReflectionTypeLoadException ex)
            //{
            //    StringBuilder sb = new StringBuilder();
            //    foreach (Exception exSub in ex.LoaderExceptions)
            //    {
            //        sb.AppendLine(exSub.Message);
            //        FileNotFoundException exFileNotFound = exSub as FileNotFoundException;
            //        if (exFileNotFound != null)
            //        {
            //            if (!string.IsNullOrEmpty(exFileNotFound.FusionLog))
            //            {
            //                sb.AppendLine("Fusion Log:");
            //                sb.AppendLine(exFileNotFound.FusionLog);
            //            }
            //        }

            //        sb.AppendLine();
            //    }
            //}
            //catch (Exception ex)
            //{

            //}

            AddVariationEvent();
        }

        /// <summary>
        /// Resets the module into an uninitialized state.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <remarks>
        /// <para>
        /// This method is usually not called when running under a web application since the web app may be shut down very
        /// abruptly, but your module should still implement it properly since it will make integration and unit testing
        /// much simpler.
        /// </para>
        /// <para>
        /// Any work done by <see cref="M:EPiServer.Framework.IInitializableModule.Initialize(EPiServer.Framework.Initialization.InitializationEngine)" /> as well as any code executing on <see cref="E:EPiServer.Framework.Initialization.InitializationEngine.InitComplete" /> should be reversed.
        /// </para>
        /// </remarks>
        public void Uninitialize(InitializationEngine context)
        {

        }

        /// <summary>
        /// Determine if the page should be indexed.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns>True/False depending on if the page should be indexed or not.</returns>
        private bool ShouldIndexSitePageData(ICanBeSearched item)
        {
            var page = item as PageData;

            if (page == null) return false;

            // Check if the page is published
            var shouldIndex = page.CheckPublishedStatus(PagePublishedStatus.Published) && !page.IsDeleted;

            // If the page should not be indexed, try to delete it if it exists in the index
            if (!shouldIndex)
            {
                IEnumerable<DeleteResult> result;
                ContentIndexer.Instance.TryDelete(page, out result);
            }

            return shouldIndex;
        }

        /// <summary>
        /// Determine if the catalog item should be indexed.
        /// </summary>
        /// <param name="content">The catalog item.</param>
        /// <returns>True/False depending on if the catalog item should be indexed or not.</returns>
        private bool ShouldIndexCatalogData(CatalogContentBase content)
        {
            // Check if the content is published
            var shouldIndex = content.StopPublish > DateTime.Now && content.StartPublish < DateTime.Now && content.Status == VersionStatus.Published;

            // If the product should not be indexed, try to delete it if it exists in the index
            if (!shouldIndex)
            {
                IEnumerable<DeleteResult> result;
                ContentIndexer.Instance.TryDelete(content, out result);
            }

            return shouldIndex;
        }

        private void AddVariationEvent()
        {
            Event ev = Event.Get(CatalogKeyEventBroadcaster.CatalogKeyEventGuid);
            ev.Raised += CatalogKeyEventUpdated;
        }

        private void CatalogKeyEventUpdated(object sender, EventNotificationEventArgs e)
        {
            var eventArgs = (CatalogKeyEventArgs)DeSerialize((byte[])e.Param);
            var inventoryUpdatedEventArgs = eventArgs as InventoryUpdateEventArgs;
            if (inventoryUpdatedEventArgs != null)
            {
                DoReIndexProducts(inventoryUpdatedEventArgs.CatalogKeys);
            }
        }

        private void DoReIndexProducts(IEnumerable<CatalogKey> catalogKeys)
        {
            var referenceConverter = ServiceLocator.Current.GetInstance<ReferenceConverter>();
            var repository = ServiceLocator.Current.GetInstance<IContentRepository>();
            var productsToIndex = new List<ProductContent>();
            foreach (var variationCode in catalogKeys.Select(x => x.CatalogEntryCode).Distinct())
            {
                var variationRef = referenceConverter.GetContentLink(variationCode);
                if (!ContentReference.IsNullOrEmpty(variationRef))
                {
                    var variation = repository.Get<IContent>(variationRef) as VariationContent;
                    if (variation != null)
                    {
                        var productRefs = variation.GetParentProducts();
                        var products = repository.GetItems(productRefs, new LoaderOptions()).OfType<ProductContent>();
                        if (products.Any())
                        {
                            productsToIndex.AddRange(products);
                        }
                    }
                }
            }

            if (productsToIndex.Any())
            {
                SearchClient.Instance.Index(productsToIndex.DistinctBy(p => p.Code));
            }
        }

        protected virtual CatalogKeyEventArgs DeSerialize(byte[] buffer)
        {
            var formatter = new BinaryFormatter();
            var stream = new MemoryStream(buffer);
            return formatter.Deserialize(stream) as CatalogKeyEventArgs;
        }
    }
}