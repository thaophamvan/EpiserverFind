using System;
using System.Collections;
using System.Collections.Generic;

namespace Core.Querying.Find.Models.Response
{
    public class PagedList<T> : IPagedList<T>
    {
        /// <summary>
        /// 	The subset of items contained only within this one page of the superset.
        /// </summary>
        protected readonly List<T> Subset = new List<T>();

        public int TotalItemCount { get; protected set; }
        public int TotalPageCount { get; protected set; }
        public int PageNumber { get; protected set; }
        public int PageSize { get; protected set; }
        public bool HasPreviousPage { get; protected set; }
        public bool HasNextPage { get; protected set; }

        public PagedList(IEnumerable<T> subset, int pageNumber, int pageSize, int totalItemCount)
        {
            PageSize = pageSize;
            PageNumber = (pageNumber == int.MaxValue) ? TotalPageCount : pageNumber;

            // set source to blank list if superset is null to prevent exceptions
            TotalItemCount = totalItemCount;
            TotalPageCount = TotalItemCount > 0
                        ? (int)Math.Ceiling(TotalItemCount / (double)PageSize)
                        : 0;

            HasPreviousPage = PageNumber > 1;
            HasNextPage = PageNumber < TotalPageCount;

            // add items to internal list
            if (subset != null && TotalItemCount > 0)
                Subset.AddRange(subset);
        }

        public PagedList(IEnumerable<T> subset, IPagedList copy)
        {
            PageSize = copy.PageSize;
            PageNumber = (copy.PageNumber == int.MaxValue) ? TotalPageCount : copy.PageNumber;

            // set source to blank list if superset is null to prevent exceptions
            TotalItemCount = copy.TotalItemCount;
            TotalPageCount = TotalItemCount > 0
                        ? (int)Math.Ceiling(TotalItemCount / (double)PageSize)
                        : 0;

            HasPreviousPage = PageNumber > 1;
            HasNextPage = PageNumber < TotalPageCount;

            // add items to internal list
            if (subset != null && TotalItemCount > 0)
                Subset.AddRange(subset);
        }

        #region IPagedList<T> Members
        public IEnumerator<T> GetEnumerator()
        {
            return Subset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public T this[int index]
        {
            get { return Subset[index]; }
        }

        public virtual int Count
        {
            get { return Subset.Count; }
        }
        #endregion

        public static PagedList<T> Empty()
        {
            return new PagedList<T>(null, 1, 1, 0);
        }
    }
}