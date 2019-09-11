using System.Configuration;
using Core.Querying.ExpressionBuilder.Interfaces;
using Core.Querying.ExpressionBuilder.Models.Response;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.Find;

namespace Core.Querying.Services
{
    public interface IMultipleSearchProvider
    {
        SearchResponse<TContent> GenericSearch<TContent>(ISearchRequest request) where TContent : IContent;
    }
}
