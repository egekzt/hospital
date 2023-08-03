using System;
using System.Collections.Generic;
using System.Linq;

namespace hospital.Models
{
    public class PaginatedList<T> : List<T>, IPagedList<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public PaginatedList(IEnumerable<T> source, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(source.Count() / (double)pageSize);
            this.AddRange(source.Skip((pageIndex - 1) * pageSize).Take(pageSize));
        }

        public bool HasPreviousPage => PageIndex > 1;

        public bool HasNextPage => PageIndex < TotalPages;

        public static PaginatedList<T> Create(IEnumerable<T> source, int pageIndex, int pageSize)
        {
            return new PaginatedList<T>(source, pageIndex, pageSize);
        }
    }

    public interface IPagedList<T> : IList<T>
    {
        int PageIndex { get; }
        int TotalPages { get; }
        bool HasPreviousPage { get; }
        bool HasNextPage { get; }
    }
}
