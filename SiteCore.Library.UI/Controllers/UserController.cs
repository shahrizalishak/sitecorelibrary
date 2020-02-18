using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SiteCore.Library.BAL.Interfaces;
using SiteCore.Library.BAL.Services;
using SiteCore.Library.BAL.Entities;
using SiteCore.Library.DAL;
using SiteCore.Library.UI.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SiteCore.Library.UI.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly UserService userService;

        public UserController(IConfiguration configuration)
        {
            userRepository = new UserRepository(configuration);
            userService = new UserService(userRepository);
        }

        public ActionResult UserList()
        {
            var users = userService.GetAll();
            var userViewModel = new UserViewModel()
            {
                Users = users
            };
            return View(userViewModel);
        }

        // GET: Book/Details/5
        public ActionResult Details(int id)
        {
            var user = userService.GetById(id);
            return View(user);
        }

        // GET: Book/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Book/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    userService.CreateUser(user);

                    return RedirectToAction(nameof(UserList));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = ex.Message;
                    return View();
                }
            }

            return View(user);

        }

        // GET: Book/Edit/5
        public ActionResult Edit(int id)
        {
            var user = userService.GetById(id);
            return View(user);
        }

        // POST: Book/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, User user)
        {
            try
            {
                userService.UpdateUser(id, user);
                return RedirectToAction(nameof(UserList));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        // GET: Book/Delete/5
        public ActionResult Delete(int id)
        {
            var user = userService.GetById(id);
            return View(user);
        }

        // POST: Book/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Book book)
        {
            try
            {
                userService.DeleteUser(id);
                return RedirectToAction(nameof(UserList));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }
    }
}
