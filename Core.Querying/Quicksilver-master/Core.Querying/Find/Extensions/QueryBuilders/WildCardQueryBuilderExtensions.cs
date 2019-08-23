using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using EPiServer.Find;
using EPiServer.Find.Api.Querying.Queries;

namespace Core.Querying.Find.Extensions.QueryBuilders
{
    public static class WildCardQueryBuilderExtensions
    {
        public static ITypeSearch<T> WildCardQuery<T>(this ITypeSearch<T> search, string query, Expression<Func<T, string>> fieldSelector, double? boost = null)
        {
            string fieldNameForAnalyzed = search.Client.Conventions.FieldNameConvention.GetFieldNameForAnalyzed(fieldSelector);
            return search.WildCardQuery(query, fieldNameForAnalyzed, boost);
        }

        public static ITypeSearch<T> WildCardQuery<T>(this ITypeSearch<T> search, string query, string fieldName, double? boost = null)
        {
            WildcardQuery wildcardQuery = new WildcardQuery(fieldName, "*" + query + "*");
            wildcardQuery.Boost = boost;
            return new Search<T, WildcardQuery>(search, delegate (ISearchContext context)
            {
                if (context.RequestBody.Query != null)
                {
                    BoolQuery boolQuery = new BoolQuery();
                    boolQuery.Should.Add(context.RequestBody.Query);
                    boolQuery.Should.Add(wildcardQuery);
                    boolQuery.MinimumNumberShouldMatch = 1;
                    context.RequestBody.Query = boolQuery;
                    return;
                }

                context.RequestBody.Query = wildcardQuery;
            });
        }

        public static ITypeSearch<T> ForWithWildcards<T>(this ITypeSearch<T> search, 
            string query, params (Expression<Func<T, string>>, double?)[] fieldSelectors)
        {
            return search
                .For(query)
                .InFields(fieldSelectors.Select(x => x.Item1).ToArray())
                .ApplyBestBets()
                .WildcardSearch(query, fieldSelectors);
        }

        public static ITypeSearch<T> WildcardSearch<T>(this ITypeSearch<T> search,
            string query, params (Expression<Func<T, string>>, double?)[] fieldSelectors)
        {
            if (string.IsNullOrWhiteSpace(query))
                return search;

            query = query.ToLowerInvariant().Replace('\'', '*');

            var words = query.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries)
                .Select(WrapInAsterisks)
                .ToList();

            var wildcardQueries = new List<WildcardQuery>();

            foreach (var fieldSelector in fieldSelectors)
            {
                string fieldName = search.Client.Conventions
                    .FieldNameConvention
                    .GetFieldNameForAnalyzed(fieldSelector.Item1);

                foreach (var word in words)
                {
                    wildcardQueries.Add(new WildcardQuery(fieldName, word)
                    {
                        Boost = fieldSelector.Item2
                    });
                }
            }

            return new Search<T, WildcardQuery>(search, context =>
            {
                var boolQuery = new BoolQuery();

                if (context.RequestBody.Query != null)
                {
                    boolQuery.Should.Add(context.RequestBody.Query);
                }

                foreach (var wildcardQuery in wildcardQueries)
                {
                    boolQuery.Should.Add(wildcardQuery);
                }

                boolQuery.MinimumNumberShouldMatch = 1;
                context.RequestBody.Query = boolQuery;
            });
        }

        public static string WrapInAsterisks(string input)
        {
            return string.IsNullOrWhiteSpace(input) ? "*" : $"*{input.Trim().Trim('*')}*";
        }
    }
}