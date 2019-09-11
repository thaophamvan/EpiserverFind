using System;
using System.Collections.Generic;
using System.Globalization;
using Core.Querying;
using EPiServer.Find;
using EPiServer.Find.Framework;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Core.Querying.ExpressionBuilder.Common;
using Core.Querying.ExpressionBuilder.Models;
using Core.Querying.Extensions;
using EPiServer.Core;
using EPiServer.Reference.Commerce.Site.Features.Product.Models;
using Mediachase.Commerce.Catalog;
using Core.Querying.ExpressionBuilder.Generics;
using Core.Querying.ExpressionBuilder.Helpers;
using Core.Querying.ExpressionBuilder.Interfaces;
using Core.Querying.ExpressionBuilder.Models.Request;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.SpecializedProperties;
using EPiServer.Find.Cms;

namespace EPiServer.Reference.Commerce.Site.Features.Test
{
    public class TestController : Controller
    {
        private readonly ITicketProvider _ticketProvider;
        private readonly IClient _client;
        private readonly IContentLoader _contentLoader;
        private readonly ReferenceConverter _referenceConverter;
        public TestController(ITicketProvider ticketProvider, IContentLoader contentLoader, ReferenceConverter referenceConverter)
        {
            _ticketProvider = ticketProvider;
            _client = SearchClient.Instance;
            _contentLoader = contentLoader;
            _referenceConverter = referenceConverter;
        }

        // GET: Test
        public async Task<ActionResult> Index()
        {
            //shoes
            var contentLink = _referenceConverter.GetContentLink("shoes", CatalogContentType.CatalogNode);
            var cate = ContentReference.IsNullOrEmpty(contentLink) ? null : _contentLoader.Get<FashionNode>(contentLink, new CultureInfo("en"));
            //ContentReference rf = new ContentReference(1073741832);
            //var page = _contentLoader.Get<FashionNode>(rf, new CultureInfo("en"));
            //var result = ContentDataQueryHandler.Instance.Create().Search<FashionProduct>()
            //    .IncludeContentUnder(new List<string>() { cate.ContentLink.ToString()}).GetContentResultSafe();

            
            //var result = ContentDataQueryHandler.Instance.Create().Search<FashionProduct>()
            //    .GetContentResultSafe();

            SearchRequest request = new SearchRequest();
            //FilterStatementItem ft = new FilterStatementItem();
            //ft.Parameters.Add(new DynamicParameter(){Value = "shoes", ParameterType = typeof(string)});
            //ft.Connector = FilterStatementConnector.And;
            //ft.PropertyId = "Code";
            //ft.OperationValue = Operation.Match;
            //ft.PropertyType = typeof(string);

            //request.Filters.Items.Add(ft);
            //var rs = ContentDataQueryHandler.Instance.Create().Search<FashionNode>().Filter(request).GetContentResultSafe();

            //request.Facets.Items.Add(ft);

            var rs = ContentDataQueryHandler.Instance.Create().Search<FashionProduct>().TermsFacetFor(p => p.AvailableColors).GetContentResult();
            var facet = rs.TermsFacetFor(x => x.AvailableColors).Terms;

            request.Facets.Items.Add(new FacetItem()
            {
                PropertyId = "AvailableColors",
                PropertyType = typeof(IEnumerable<string>),
                OperationValue = FacetOperation.TermsFacetFor,
            });
            var rs1 = ContentDataQueryHandler.Instance.Create().Search<FashionProduct>().AddFacetFor(request).GetContentResultSafe();

            var facet1 = rs1.TermsFacetFor(x => x.AvailableColors).Terms;

            await Task.FromResult(facet1);
            return null;

        }

        //private async Task<SearchResults<Employee>> SearchAsync(ITicketProvider ticketProvider, int i)
        //{
        //    return await _client.Search<Employee>().Filter(x => x.DateOfBirth.After(DateTime.Now)).;

        //}
    }
}