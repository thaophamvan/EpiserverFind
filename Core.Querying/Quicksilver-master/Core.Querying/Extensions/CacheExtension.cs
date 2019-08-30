using System.Security.Cryptography;
using System.Text;
using Core.Querying.Find.Models.Request;

namespace Core.Querying.Extensions
{
    public static class CacheExtension
    {
        public static string GetCacheKey(this ISearchRequest request)
        {
            var marketId = request.MarketId;
            var useWildCardSearch = request.UseWildCardSearch;
            var searchTerm = request.SearchTerm;
            var filterSearchTerm = request.FilterSearchTerm;
            var pageNumber = request.PageNumber;
            var pageSize = request.PageSize;
            
            string searchTermFields = "";
            foreach (var pair in request.SearchTermFields)
            {
                searchTermFields = $"{searchTermFields}-{pair.Key}:{pair.Value}";
            }
            var filterSpecificationLanguage = request.Filters.Language.Name;
            var filterSpecificationCultureInfo = request.Filters.CultureInfo.Name;

            var filterSpecificationITem = "";
            foreach (var item in request.Filters.Items)
            {
                filterSpecificationITem = $"{filterSpecificationITem}-{item.Field}";
            }

            var sorts = "";
            foreach (var item in request.Sorts.Items)
            {
                sorts = $"{sorts}-{item.Field}:{item.Ascending}";
            }

            var facet = "";
            foreach (var item in request.Facets.Items)
            {
                sorts = $"{facet}-{item.Field}:{item.DataType}";
            }

            var cacheKey = $"{marketId}_{useWildCardSearch}_{searchTerm}_{filterSearchTerm}_{pageNumber}_{pageSize}" +
                           $"_{filterSpecificationLanguage}_{filterSpecificationCultureInfo}_{sorts}";
            return cacheKey.Md5Hash();
        }

        private static string Md5Hash(this string input)
        {
            byte[] hash;
            using (MD5 md5 = MD5.Create())
            {
                hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}

