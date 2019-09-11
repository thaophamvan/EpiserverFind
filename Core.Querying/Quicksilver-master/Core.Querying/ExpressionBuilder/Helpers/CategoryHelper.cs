using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.ServiceLocation;

namespace Core.Querying.ExpressionBuilder.Helpers
{
    public static class CategoryHelper
    {
        private static readonly Injected<CategoryRepository> _categoryRepository;

        public static string GetDisplayName(int categoryId)
        {
            var cat = _categoryRepository.Service.Get(categoryId);
            return cat == null ? string.Empty : cat.Description;
        }

        public static string GetDisplayName(CategoryList categoryId)
        {
            if (categoryId != null && categoryId.Any())
            {
                var catId = categoryId.FirstOrDefault();
                return GetDisplayName(catId);
            }

            return string.Empty;
        }

        public static IEnumerable<string> GetDisplayNames(IEnumerable<int> categoryIds)
        {
            var names = new List<string>();
            if (categoryIds != null && categoryIds.Any())
            {
                foreach (var id in categoryIds)
                {
                    var name = GetDisplayName(id);
                    if (!string.IsNullOrEmpty(name))
                    {
                        names.Add(name);
                    }
                }
            }

            return names;
        }

        public static IEnumerable<string> GetDisplayNamesRespectOrder(CategoryList categoryIds)
        {
            var catList = new List<Category>();
            if (categoryIds != null && categoryIds.Any())
            {
                foreach (var id in categoryIds)
                {
                    var cat = _categoryRepository.Service.Get(id);
                    if (cat != null)
                    {
                        catList.Add(cat);
                    }
                }
            }

            return catList.Any() ? catList.OrderBy(x => x.SortOrder).Select(x => x.Description) : Enumerable.Empty<string>();
        }

        public static IList<Category> GetCategoriesForRoot(string rootCategoryName)
        {
            var root = _categoryRepository.Service.Get(rootCategoryName);
            if (root != null)
            {
                return root.Categories.ToList();
            }

            return new List<Category>();
        }

        public static IDictionary<int, string> GetCategoryDictionary(IEnumerable<int> categoryIds)
        {
            var dictionary = new Dictionary<int, string>();
            if (categoryIds != null && categoryIds.Any())
            {
                foreach (var id in categoryIds)
                {
                    var name = GetDisplayName(id);
                    if (!string.IsNullOrEmpty(name))
                    {
                        dictionary.Add(id, name);
                    }
                }
            }

            return dictionary;
        }

        public static IDictionary<int, string> GetCategoryDictionaryForRoot(int rootCategoryId)
        {
            var dictionary = new Dictionary<int, string>();

            var root = _categoryRepository.Service.Get(rootCategoryId);
            if (root != null)
            {
                foreach (var childCategory in root.Categories)
                {
                    var name = GetDisplayName(childCategory.ID);
                    if (!string.IsNullOrEmpty(name))
                    {
                        dictionary.Add(childCategory.ID, name);
                    }
                }

            }

            return dictionary;
        }

        public static IDictionary<int, string> GetCategoryDictionaryForRoot(string rootCategoryName)
        {
            var dictionary = new Dictionary<int, string>();

            var root = _categoryRepository.Service.Get(rootCategoryName);
            if (root != null)
            {
                foreach (var childCategory in root.Categories)
                {
                    var name = GetDisplayName(childCategory.ID);
                    if (!string.IsNullOrEmpty(name))
                    {
                        dictionary.Add(childCategory.ID, name);
                    }
                }

            }

            return dictionary;
        }

        public static IEnumerable<string> GetDisplayNamesByRootCategoryName(string rootCategoryName)
        {
            var names = new List<string>();

            var root = _categoryRepository.Service.Get(rootCategoryName);
            if (root != null && root.Categories.Any())
            {
                foreach (var child in root.Categories)
                {
                    var name = GetDisplayName(child.ID);
                    if (!string.IsNullOrEmpty(name))
                    {
                        names.Add(name);
                    }
                }
            }

            return names;
        }

        public static Category GetCategory(int categoryId)
        {
            return _categoryRepository.Service.Get(categoryId);
        }

        public static Category GetCategoryByFriendlyName(string categoryFriendlyName)
        {
            // Start at the root
            var root = _categoryRepository.Service.Get(Category.RootName);
            if (root != null && root.Categories.Any())
            {
                return FindCategoryRecursive(root, categoryFriendlyName);
            }

            return null;
        }

        public static Category FindCategoryRecursive(Category category, string categoryFriendlyName)
        {
            foreach (var cat in category.Categories)
            {
                if (cat.Description.Equals(categoryFriendlyName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return cat;
                }

                if (cat.Categories != null && cat.Categories.Any())
                {
                    var foundCat = FindCategoryRecursive(cat, categoryFriendlyName);
                    if (foundCat != null)
                    {
                        return foundCat;
                    }
                }
            }

            return null;
        }

        public static Category GetCategory(CategoryList categoryId)
        {
            if (categoryId != null && categoryId.Any())
            {
                return _categoryRepository.Service.Get(categoryId.FirstOrDefault());
            }

            return null;
        }
    }
}
