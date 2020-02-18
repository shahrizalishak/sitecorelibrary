using System;
using System.Collections.Generic;
using SiteCore.Library.BAL.Entities;

namespace SiteCore.Library.BAL.Interfaces
{
    public interface IUserRepository
    {
        void Create(User user);
        void Update(int id, User user);
        void Delete(int userId);
        User GetById(int userId);
        IList<User> GetAll();
    }
}
