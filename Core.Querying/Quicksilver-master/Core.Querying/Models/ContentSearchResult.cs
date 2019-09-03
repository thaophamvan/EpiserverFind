using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPiServer.Find.Cms;

namespace Core.Querying.Models
{
    public class ContentSearchResult<TContent>
    {
        private IEnumerable<TContent> Items { get; set; }
        private int TotalMatching { get; set; }
    }
}
