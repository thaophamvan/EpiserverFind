using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Querying.Find.Models.Request;
using Core.Querying.Find.Models.Response;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.Find;

namespace Core.Querying.Services
{
    public class DatabaseSearchServices : ISearchServices
    {
        public SearchResponse<TEntry> GenericSearch<TEntry>(ISearchRequest request) where TEntry : IContent
        {
            throw new NotImplementedException();
        }
    }
}
