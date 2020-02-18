using System;
using System.ComponentModel.DataAnnotations;

namespace SiteCore.Library.BAL.Entities
{
    public class User
    { 
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        public int MobileNo { get; set; }
    }
}
