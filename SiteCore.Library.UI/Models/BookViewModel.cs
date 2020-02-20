using System;
using System.Collections.Generic;
using System.Linq;
using SiteCore.Library.BAL.Entities;

namespace SiteCore.Library.UI.Models
{
    public class BookViewModel
    {
        public BookViewModel(List<Book> books, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            Books = books;
            AvailableAuthors = new List<Author>();
            SelectedAuthors = new List<int>();
        }

        public List<Book> Books { get; set; }
        public List<Author> AvailableAuthors { get; set; }
        public List<int> SelectedAuthors { get; set; }
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }

        public static BookViewModel Create(List<Book> books, int pageIndex, int pageSize)
        {
            var count = books.Count;
            var items = books.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new BookViewModel(items, count, pageIndex, pageSize);
        }
    }
}
