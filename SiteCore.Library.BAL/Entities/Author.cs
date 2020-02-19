using System;
using System.ComponentModel.DataAnnotations;

namespace SiteCore.Library.BAL.Entities
{
    public class Author
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
