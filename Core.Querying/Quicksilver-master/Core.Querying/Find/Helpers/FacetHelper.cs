using System.Collections.Generic;
using System.Linq;
using Core.Querying.Find.Extensions.FacetBuilders;
using Core.Querying.Find.FacetRegistry;
using Core.Querying.Find.Models.Request;
using Core.Querying.Find.Models.Response;
using Core.Querying.Shared;
using EPiServer.Core;
using EPiServer.Find;
using EPiServer.Find.Api.Facets;
using EPiServer.ServiceLocation;

namespace Core.Querying.Find.Helpers
{
    public static class FacetHelper
    {
        public static List<FacetGroup> ExtractFacet<T>(this IHasFacetResults<T> result, FacetSpecification specification, int priceInterval = 0) where T : IContent
        {
            if (specification.Items == null) return null;

            var termFacets = result.ExtractTermsFacetFor(specification.Items.Where(f => f.GetType() == typeof(FacetItem)));
            var dateFacets = result.ExtractRangeDateFacetFor(specification.Items.Where(f => f.GetType() == typeof(DateFacetItem)).OfType<DateFacetItem>());
            var numericFacet = result.ExtractRangeNumericFacetFor(specification.Items.Where(f => f.GetType() == typeof(NumericFacetItem)).OfType<NumericFacetItem>(), priceInterval);

            return termFacets.Concat(dateFacets).Concat(numericFacet).ToList();
        }

        public static List<FacetGroup> ExtractFacet1<T>(this IHasFacetResults<T> result, FacetSpecification specification, int priceInterval = 0) where T : IContent
        {
            if (specification.Items == null) return null;

            var termFacets = result.ExtractTermsFacetFor(specification.Items.Where(f => f.GetType() == typeof(FacetItem)));
            var dateFacets = result.ExtractRangeDateFacetFor(specification.Items.Where(f => f.GetType() == typeof(DateFacetItem)).OfType<DateFacetItem>());
            var numericFacet = result.ExtractRangeNumericFacetFor(specification.Items.Where(f => f.GetType() == typeof(NumericFacetItem)).OfType<NumericFacetItem>(), priceInterval);

            return termFacets.Concat(dateFacets).Concat(numericFacet).ToList();
        }

        public static List<FacetGroup> ExtractTermsFacetFor<T>(this IHasFacetResults<T> result, IEnumerable<FacetItem> facets) where T : IContent
        {
            var groups = new List<FacetGroup>();

            if (result.Facets == null) return groups;
            var facetRegistry = ServiceLocator.Current.GetInstance<IFacetRegistry>();

            foreach (var facet in facets)
            {
                var facetResult = result.Facets.FirstOrDefault(f => f.Name == facet.Field) as TermsFacet;
                var facetItems = new List<FacetFilter>();

                facetItems.Add(new FacetFilter
                {
                    GroupKey = facet.Field,
                    Name = GlobalConstants.DefaultValues.All,
                    Key = GlobalConstants.DefaultValues.All,
                    Count = facetResult.Sum(x => x.Count),
                    SortOrder = 0
                });

                // Check if there is an order for this facet (setup in Categories)
                var categories = CategoryHelper.GetCategoriesForRoot($"Facet_{facet.Field}");
                if (categories.Any())
                {
                    foreach (var item in facetResult)
                    {
                        var term = item.Term;
                        if (facetRegistry.IsTermFacetWithSpecificDataType(facet.Field, GlobalConstants.FacetDataTypes.Boolean))
                        {
                            term = term == GlobalConstants.DefaultValues.T ? GlobalConstants.DefaultValues.Yes : GlobalConstants.DefaultValues.No;
                        }

                        // Find the filter "Term" in categories and get the sort order
                        var sortOrder = 1;
                        var foundCategory = categories.FirstOrDefault(c => c.Description.Equals(term));
                        if (foundCategory != null)
                        {
                            sortOrder = foundCategory.SortOrder;
                        }

                        facetItems.Add(new FacetFilter
                        {
                            GroupKey = facet.Field,
                            Name = term,
                            Key = term,
                            Count = item.Count,
                            SortOrder = sortOrder 
                        });
                    }
                }
                else
                {
                    // Order by the filter "Term"
                    var sortOrder = 1;
                    foreach (var item in facetResult.OrderBy(x => x.Term))
                    {
                        var term = item.Term;
                        if (facetRegistry.IsTermFacetWithSpecificDataType(facet.Field, GlobalConstants.FacetDataTypes.Boolean))
                        {
                            term = term == GlobalConstants.DefaultValues.T ? GlobalConstants.DefaultValues.Yes : GlobalConstants.DefaultValues.No;
                        }

                        facetItems.Add(new FacetFilter
                        {
                            GroupKey = facet.Field,
                            Name = term,
                            Key = term,
                            Count = item.Count,
                            SortOrder = sortOrder
                        });

                        sortOrder++;
                    }
                }

                groups.Add(new FacetGroup
                {
                    GroupName = facet.Field,
                    Items = facetItems.OrderBy(x => x.SortOrder).ToList()
                });
            }

            return groups;
        }

        public static List<FacetGroup> ExtractRangeDateFacetFor<T>(this IHasFacetResults<T> result, IEnumerable<DateFacetItem> facets) where T : IContent
        {
            var groups = new List<FacetGroup>();

            if (result.Facets == null) return groups;

            foreach (var facet in facets)
            {
                var facetResult = result.Facets.FirstOrDefault(f => f.Name == facet.Field) as DateRangeFacet;
                groups.Add(new FacetGroup
                {
                    GroupName = facet.Field,
                    Items = facetResult.Select(r => new FacetFilter
                    {
                        Key = $"{r.From} {r.To}",
                        Count = r.TotalCount
                    }).ToList()
                });
            }

            return groups;
        }

        public static List<FacetGroup> ExtractRangeNumericFacetFor<T>(this IHasFacetResults<T> result, IEnumerable<NumericFacetItem> facets, int priceInterval = 0) where T : IContent
        {
            var groups = new List<FacetGroup>();
            foreach (var facet in facets)
            {
                if (facet.Field == FacetBuilderExtensions.PreDefineFacetField.PRICE)
                {
                    var facetData = result.Facets.FirstOrDefault(f => f is HistogramFacet) as HistogramFacet;
                    if (facetData != null)
                    {
                        if (priceInterval == 0)
                        {
                            priceInterval = GlobalConstants.DefaultValues.DefaultPriceInterval;
                        }
                        groups.Add(new FacetGroup
                        {
                            GroupName = facet.Field,
                            Items = facetData.Entries.Select(e => new FacetFilter
                            {
                                GroupKey = facet.Field,
                                Key = $"{e.Key}{GlobalConstants.Signs.Hyphen}{(e.Key + priceInterval)}",
                                Name = $"${e.Key} {GlobalConstants.Signs.Hyphen} ${(e.Key + priceInterval)}",
                                Count = e.Count
                            }).ToList()
                        });
                    }
                }
                else
                {
                    var facetResult = result.Facets.FirstOrDefault(f => f.Name == facet.Field) as NumericRangeFacet;
                    groups.Add(new FacetGroup
                    {
                        GroupName = facet.Field,
                        Items = facetResult.Select(r => new FacetFilter
                        {
                            GroupKey = facet.Field,
                            Key = $"{r.From}{GlobalConstants.Signs.Hyphen}{r.To}",
                            Name = $"${r.From} - ${r.To}",
                            Count = r.TotalCount
                        }).ToList()
                    });
                }
            }

            return groups;
        }
    }
}
