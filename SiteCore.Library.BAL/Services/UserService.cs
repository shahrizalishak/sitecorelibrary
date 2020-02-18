using System;
using System.Collections.Generic;
using System.Linq;
using SiteCore.Library.BAL.Entities;
using SiteCore.Library.BAL.Interfaces;

namespace SiteCore.Library.BAL.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public List<User> GetAll()
        {
            return _userRepository.GetAll().ToList();
        }

        public void CreateBook(User user)
        {
            _userRepository.Create(user);
        }

        public User GetById(int id)
        {
            var user = _userRepository.GetById(id);
            return user;
        }

        public void UpdateBook(int id, User user)
        {
            _userRepository.Update(id, user);
        }

        public void DeleteBook(int id)
        {
            _userRepository.Delete(id);
        }
    }
}
