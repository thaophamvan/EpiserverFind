using System.Collections.Generic;
using EPiServer.Core;

namespace Core.Querying.Models
{
    public interface ICategorizableContent
    {
         IList<ContentReference> Categories { get; set; } 
    }
}