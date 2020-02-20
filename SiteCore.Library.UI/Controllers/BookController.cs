using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SiteCore.Library.BAL.Entities;
using SiteCore.Library.BAL.Interfaces;
using SiteCore.Library.BAL.Services;
using SiteCore.Library.DAL;
using SiteCore.Library.UI.Models;

namespace SiteCore.Library.UI.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookRepository bookRepository;
        private readonly BookService bookService;

        public BookController(IConfiguration configuration)
        {
            bookRepository = new BookRepository(configuration);
            bookService = new BookService(bookRepository);
        }

        // GET: Book
        public ActionResult Index(int? pageNumber, string sortOrder)
        {
            int pageSize = 2;
            ViewData["CurrentSort"] = sortOrder;
            ViewData["BookSortParm"] = String.IsNullOrEmpty(sortOrder) ? "book_desc" : "";
            ViewData["AuthorSortParm"] = sortOrder == "Author" ? "author_desc" : "Author";

            var books = bookService.GetAll().AsQueryable();

            switch (sortOrder)
            {
                case "book_desc":
                    books = books.OrderByDescending(b => b.Title);
                    break;
                case "Author":
                    books = books.OrderBy(b => b.AuthorList);
                    break;
                case "author_desc":
                    books = books.OrderByDescending(b => b.AuthorList);
                    break;
                default:
                    books = books.OrderBy(b => b.Title);
                    break;
            }

            var bookViewModel = BookViewModel.Create(books.ToList(), pageNumber ?? 1, pageSize);
            return View(bookViewModel);
        }

        // GET: Book/Details/5
        public ActionResult Details(int id)
        {
            var books = bookService.GetByIdNew(id);
            //var books = bookService.GetAuthorsById(id);
            var bookViewModel = new DetailsBookViewModel()
            {
                Books = books
            };
            return View(bookViewModel);
        }

        // GET: Book/Create
        public ActionResult Create()
        {
            var authors = bookService.GetAuthors();

            var createBookViewModel = new CreateBookViewModel
            {
                AvailableAuthors = authors
            };

            return View(createBookViewModel);
        }

        // POST: Book/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateBookViewModel createBookViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var book = new Book()
                    {
                        Title = createBookViewModel.Book.Title
                    };

                    foreach (var authorId in createBookViewModel.SelectedAuthors)
                    {
                        book.AuthorId.Add(authorId);
                    }

                    bookService.CreateBook(book);

                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }

            return View();
        }

        // GET: Book/Edit/5
        public ActionResult Edit(int id)
        {
            var authors = bookService.GetAuthors();
            var books = bookService.GetByIdNew(id);
            //var books = bookService.GetAuthorsById(id);
            var bookViewModel = new DetailsBookViewModel()
            {
                Books = books,
                AvailableAuthors = authors
            };
            return View(bookViewModel);
        }

        // POST: Book/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Book book)
        {
            try
            {

                bookService.UpdateBook(id, book);
                return RedirectToAction(nameof(Index));
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
            var book = bookService.GetById(id);
            return View(book);
        }

        // POST: Book/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                bookService.DeleteBook(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }
    }
}