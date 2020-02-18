using System;
using System.Collections.Generic;
using SiteCore.Library.BAL.Entities;

namespace SiteCore.Library.UI.Models
{
    public class UserViewModel
    {
        public UserViewModel()
        {
        }

        public List<User> Users { get; set; }
    }
}
