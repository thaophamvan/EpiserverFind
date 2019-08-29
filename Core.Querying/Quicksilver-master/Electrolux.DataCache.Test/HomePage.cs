using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAnnotations;

namespace Electrolux.DataCache.Test
{
    [ContentType(DisplayName = "Home Page For Test", GUID = "C384ABA8-7BB7-4A84-9A91-549E40B1AD60", Description = "")]
    public class HomePageForTest : PageData
    {
        [Display(
            Name = "Custom CSS Class",
            Description = "Use to add custom CSS classes to the root elements of a page section.",
            Order = 100)]
        public virtual string CompanyName { get; set; }

    }
}