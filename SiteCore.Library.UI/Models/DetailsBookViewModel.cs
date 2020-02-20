using System;
using System.Collections.Generic;
using System.Linq;
using SiteCore.Library.BAL.Entities;

namespace SiteCore.Library.UI.Models
{
    public class DetailsBookViewModel
    {
        public DetailsBookViewModel()
        {
            AvailableAuthors = new List<Author>();
            SelectedAuthors = new List<int>();
            Books = new List<Book>();
        }

        public List<Book> Books { get; set; }
        public List<Author> AvailableAuthors { get; set; }
        public List<int> SelectedAuthors { get; set; }
        
    }
}
