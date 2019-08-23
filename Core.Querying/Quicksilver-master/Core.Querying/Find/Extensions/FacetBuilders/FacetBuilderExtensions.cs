using System;
using System.Collections.Generic;
using System.Linq;
using Core.Querying.Find.Helpers;
using Core.Querying.Find.Models.Request;
using Core.Querying.Find.SearchRequests;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.Find;
using EPiServer.Find.Api.Facets;
using EPiServer.ServiceLocation;

namespace Core.Querying.Find.Extensions.FacetBuilders
{
    public static class FacetBuilderExtensions
    {
        public class PreDefineFacetField
        {
            public const string PRICE = "Prices";
            public const string COLOUR_NAME = "ColourName";
            public const string SIZE_DESCRIPTION = "SizeDescription";
        }

        public static ITypeSearch<T> AddFacetFor<T>(this ITypeSearch<T> query, IFacetRequest request) where T : IContent
        {
            if (request?.Facets?.Items == null) return query;

            return query.TermsFacetFor(request.Facets.Items.Where(f => f.GetType() == typeof(FacetItem)))
                        .RangeDateFacetFor(request.Facets.Items.Where(f => f.GetType() == typeof(DateFacetItem)).OfType<DateFacetItem>())
                        .RangeNumericFacetFor(request.Facets.Items.Where(f => f.GetType() == typeof(NumericFacetItem)).OfType<NumericFacetItem>());
        }

        public static ITypeSearch<T> AddFacetFor<T>(this ITypeSearch<T> query, IProductSearchRequest request) where T : ProductContent
        {
            if (request?.Facets?.Items == null) return query;

            var termFacets = request.Facets.Items.Where(f => f.GetType() == typeof(FacetItem));
            foreach(var facet in termFacets)
            {
                switch (facet.Field)
                {
                    default:
                        var lambaExpression = PropertyInfoHelper.GetLamdaExpression<T>(facet.Field);
                        if (lambaExpression != null)
                        {
                            query = TypeSearchExtensions.TermsFacetFor(query, lambaExpression, (Action<TermsFacetRequest>)(f => f.Size = 100));
                        }
                        else
                        {
                            query = TypeSearchExtensions.TermsFacetFor(query, facet.Field, f => { f.Size = 100; f.Field = facet.Field + "$$" + facet.DataType; });
                        }
                        break;
                }
            }

            var numericRangeFacets = request.Facets.Items.Where(f => f.GetType() == typeof(NumericFacetItem)).OfType<NumericFacetItem>();
            foreach (var facet in numericRangeFacets)
            {
                switch (facet.Field)
                {
                    case PreDefineFacetField.PRICE:
                        //query = TypeSearchExtensions.RangeFacetFor(query, p => p.Prices());
                        //var productQuery = query as ITypeSearch<ProductContent>;
                        //if (productQuery != null)
                        //{
                        //    var currencyCode = ServiceLocator.Current.GetInstance<ICurrencyService>().GetCurrentCurrency().CurrencyCode;
                        //    productQuery = productQuery.HistogramFacetFor(
                        //        p => p.Prices, pp => pp.Price, 
                        //        request.PriceInterval, 
                        //        pp => pp.MarketId.Match(request.MarketId) & pp.CurrencyCode.Match(currencyCode));
                        //    query = (ITypeSearch<T>)productQuery;
                        //}
                        break;
                    default:
                        var lambaExpression = PropertyInfoHelper.GetLamdaExpression<T, object>(facet.Field);
                        query = TypeSearchExtensions.RangeFacetFor(query, lambaExpression, facet.Range.ToArray());
                        break;
                }
            }

            var dateRangeFacets = request.Facets.Items.Where(f => f.GetType() == typeof(DateFacetItem)).OfType<DateFacetItem>();
            foreach (var facet in dateRangeFacets)
            {
                switch (facet.Field)
                {
                    default:
                        var lambaExpression = PropertyInfoHelper.GetLamdaExpression<T>(facet.Field);
                        query = TypeSearchExtensions.RangeFacetFor(query, lambaExpression, facet.Range.ToArray());
                        break;
                }
            }


            return query;
        }

        public static ITypeSearch<T> AddFacetForVariation<T>(this ITypeSearch<T> query, IProductSearchRequest request) where T : VariationContent
        {
            if (request?.Facets?.Items == null) return query;

            var termFacets = request.Facets.Items.Where(f => f.GetType() == typeof(FacetItem));
            foreach (var facet in termFacets)
            {
                switch (facet.Field)
                {
                    default:
                        var lambaExpression = PropertyInfoHelper.GetLamdaExpression<T>(facet.Field);
                        if (lambaExpression != null)
                        {
                            query = TypeSearchExtensions.TermsFacetFor(query, lambaExpression, (Action<TermsFacetRequest>)(f => f.Size = 100));
                        }
                        else
                        {
                            query = TypeSearchExtensions.TermsFacetFor(query, facet.Field, f => { f.Size = 100; f.Field = facet.Field + "$$" + facet.DataType; });
                        }
                        break;
                }
            }

            var numericRangeFacets = request.Facets.Items.Where(f => f.GetType() == typeof(NumericFacetItem)).OfType<NumericFacetItem>();
            foreach (var facet in numericRangeFacets)
            {
                switch (facet.Field)
                {
                    case PreDefineFacetField.PRICE:
                        //var productQuery = query as ITypeSearch<VariationContent>;
                        //if (productQuery != null)
                        //{
                        //    var currencyCode = ServiceLocator.Current.GetInstance<ICurrencyService>().GetCurrentCurrency().CurrencyCode;
                        //    productQuery = productQuery
                        //    .HistogramFacetFor(
                        //    p => p.Prices, pp => pp.Price,
                        //    request.PriceInterval,
                        //    pp => pp.MarketId.Match(request.MarketId) & pp.CurrencyCode.Match(currencyCode));
                        //    query = (ITypeSearch<T>)productQuery;
                        //}
                        break;
                    default:
                        var lambaExpression = PropertyInfoHelper.GetLamdaExpression<T, object>(facet.Field);
                        query = TypeSearchExtensions.RangeFacetFor(query, lambaExpression, facet.Range.ToArray());
                        break;
                }
            }

            var dateRangeFacets = request.Facets.Items.Where(f => f.GetType() == typeof(DateFacetItem)).OfType<DateFacetItem>();
            foreach (var facet in dateRangeFacets)
            {
                switch (facet.Field)
                {
                    default:
                        var lambaExpression = PropertyInfoHelper.GetLamdaExpression<T>(facet.Field);
                        query = TypeSearchExtensions.RangeFacetFor(query, lambaExpression, facet.Range.ToArray());
                        break;
                }
            }

            return query;
        }

        public static ITypeSearch<T> TermsFacetFor<T>(this ITypeSearch<T> query, IEnumerable<FacetItem> facets) where T : IContent
        {
            if (facets == null) return query;

            foreach (var facet in facets)
            {

                var lambaExpression = PropertyInfoHelper.GetLamdaExpression<T>(facet.Field);
                query = TypeSearchExtensions.TermsFacetFor(query, lambaExpression);
            }

            return query;
        }

        public static ITypeSearch<T> RangeDateFacetFor<T>(this ITypeSearch<T> query, IEnumerable<DateFacetItem> facets) where T : IContent
        {
            if (facets == null) return query;

            foreach (var facet in facets)
            {
                var lambaExpression = PropertyInfoHelper.GetLamdaExpression<T>(facet.Field);
                query = TypeSearchExtensions.RangeFacetFor(query, lambaExpression, facet.Range.ToArray());
            }

            return query;
        }

        public static ITypeSearch<T> RangeNumericFacetFor<T>(this ITypeSearch<T> query, IEnumerable<NumericFacetItem> facets) where T : IContent
        {
            if (facets == null) return query;

            foreach (var facet in facets)
            {
                var lambaExpression = PropertyInfoHelper.GetLamdaExpression<T, object>(facet.Field);
                query = TypeSearchExtensions.RangeFacetFor(query, lambaExpression, facet.Range.ToArray());
            }

            return query;
        }
    }
}