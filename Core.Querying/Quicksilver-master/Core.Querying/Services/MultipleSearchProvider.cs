﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Querying.Find.Models.Request;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Find;

namespace Core.Querying.Services
{
    public class MultipleSearchProvider : ServicesBase, IMultipleSearchProvider
    {
        public ITypeSearch<TEntry> GeneralSearch<TEntry>(ISearchRequest request) where TEntry : CatalogContentBase
        {
            throw new NotImplementedException();
        }

        public void Initialize(ProviderSettings providerSettings)
        {
            throw new NotImplementedException();
        }
    }
}
