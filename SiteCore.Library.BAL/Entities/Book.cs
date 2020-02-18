using System;
using System.ComponentModel.DataAnnotations;

namespace SiteCore.Library.BAL.Entities
{
    public class Book
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
    }
}
