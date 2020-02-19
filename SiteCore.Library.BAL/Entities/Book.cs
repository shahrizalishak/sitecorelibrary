using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiteCore.Library.BAL.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public IList<string> Author { get; set; }
        public IList<int> AuthorId { get; set; }
        public string AuthorList
        {
            get
            {
                return String.Join(", ", Author);
            }
        }

        public Book()
        {
            Author = new List<string>();
            AuthorId = new List<int>();
        }


    }
}
