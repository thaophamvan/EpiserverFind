using System;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;

namespace Core.Querying.Find.Extensions.Content
{
    public static class VariationExtensions
    {
        public static bool IsPublishedAndNotExpired(this VariationContent variation)
        {
            return (variation.Status == VersionStatus.Published) && (!variation.StopPublish.HasValue || variation.StopPublish.Value > DateTime.Now);
        }
        public static bool IsAvailable(this VariationContent variation)
        {
            return variation.IsPublishedAndNotExpired();
        }
    }
}