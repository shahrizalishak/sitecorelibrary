using System;
using System.Collections.Generic;
using SiteCore.Library.BAL.Entities;

namespace SiteCore.Library.UI.Models
{
    public class BookViewModel
    {
        public BookViewModel()
        {
            AuthorName = new List<int>();
        }

        public List<Book> Books { get; set; }
        public List<int> AuthorName { get; set; }
    }
}
