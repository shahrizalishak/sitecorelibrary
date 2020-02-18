using System;
using System.Collections.Generic;
using SiteCore.Library.BAL.Entities;

namespace SiteCore.Library.UI.Models
{
    public class BookViewModel
    {
        public BookViewModel()
        {
        }

        public List<Book> Books { get; set; }
    }
}
