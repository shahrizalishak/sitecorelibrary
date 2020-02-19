using System;
using System.Collections.Generic;
using SiteCore.Library.BAL.Entities;

namespace SiteCore.Library.UI.Models
{
    public class CreateBookViewModel
    {
        public CreateBookViewModel()
        {
            AvailableAuthors = new List<Author>();
            SelectedAuthors = new List<int>();
        }

        public Book Book { get; set; }
        public List<Author> AvailableAuthors { get; set; }
        public List<int> SelectedAuthors { get; set; }
    }
}