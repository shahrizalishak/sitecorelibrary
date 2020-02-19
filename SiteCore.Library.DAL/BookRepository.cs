using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.Extensions.Configuration;
using SiteCore.Library.BAL.Entities;
using SiteCore.Library.BAL.Interfaces;
using SiteCore.Library.DAL.DataModels;

namespace SiteCore.Library.DAL
{
    public class BookRepository : IBookRepository
    {
        private readonly IConfiguration _configuration;
        readonly string connectionString;

        public BookRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration["ConnectionStrings:DefaultConnection"];
        }

        public void Create(Book book)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql =
                    $"Insert Into Books (Title) Values (@Title)";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    SqlParameter titleParameter = new SqlParameter("Title", book.Title);
                    //SqlParameter authorParameter = new SqlParameter("Author", book.Author);

                    command.Parameters.Add(titleParameter);
                    //command.Parameters.Add(authorParameter);

                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();

                }

                string sqlQueryNewBook = "Select * From Books Where Title = @Title";

                SqlCommand commandQuery = new SqlCommand(sqlQueryNewBook, connection);
                SqlParameter bookTitleParameter = new SqlParameter("Title", book.Title);
                commandQuery.Parameters.Add(bookTitleParameter);

                using (SqlDataReader dataReader = commandQuery.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        book.Id = Convert.ToInt32(dataReader["Id"]);
                    }
                }

                foreach (var authorId in book.AuthorId)
                {
                    sql =
                    $"Insert Into BookAuthor (BookId, AuthorId) Values (@BookId, @AuthorId)";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        SqlParameter bookIdParameter = new SqlParameter("BookId", book.Id);
                        SqlParameter authorParameter = new SqlParameter("AuthorId", authorId);

                        command.Parameters.Add(bookIdParameter);
                        command.Parameters.Add(authorParameter);

                        command.CommandType = CommandType.Text;
                        command.ExecuteNonQuery();
                    }
                }


            }
        }

        public void Delete(int bookId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql =
                    $"Delete From Books Where Id=@Id";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    SqlParameter idParameter = new SqlParameter("Id", bookId);

                    command.Parameters.Add(idParameter);

                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
            }
        }

        public IList<Book> GetAll()
        {
            var bookList = new List<Book>();
            var bookData = new List<BookAuthor>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "select b.Id, b.Title, a.Name from Books as b JOIN BookAuthor as ba " +
                    "on b.Id = ba.BookId JOIN Authors AS a ON ba.AuthorId = a.Id";

                SqlCommand command = new SqlCommand(sql, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        bookData.Add(new BookAuthor
                        {
                            BookId = Convert.ToInt32(dataReader["Id"]),
                            BookTitle = dataReader["Title"].ToString(),
                            AuthorName = dataReader["Name"].ToString()
                        });
                    }
                }
            }

            var booksByTitle = bookData.GroupBy(b => b.BookTitle).ToList();

            foreach (var book in booksByTitle)
            {
                var record = new Book();

                foreach (var item in book)
                {
                    record.Id = item.BookId;
                    record.Title = item.BookTitle;
                    record.Author.Add(item.AuthorName);
                }

                bookList.Add(record);
            }

            return bookList;
        }

        public IList<Author> GetAllAuthors()
        {
            var authors = new List<Author>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT Id, Name FROM Authors";

                SqlCommand command = new SqlCommand(sql, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        authors.Add(new Author
                        {
                            Id = Convert.ToInt32(dataReader["Id"]),
                            Name = dataReader["Name"].ToString()
                        });
                    }
                }
            }

            return authors;
        }

        public IList<Author> GetAuthorsById(int id)
        {
            var authors = new List<Author>();
            var authorId = new List<int>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sqlAuthorId = "SELECT AuthorId FROM BookAuthor where BookId = @Id";

                SqlCommand commandAuthorId = new SqlCommand(sqlAuthorId, connection);
                SqlParameter ParameterId = new SqlParameter("Id", id);
                commandAuthorId.Parameters.Add(ParameterId);
                using (SqlDataReader dataReader = commandAuthorId.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        authorId.Add(Convert.ToInt32(dataReader["AuthorId"]));

                    }
                }


                foreach (var authoridnew in authorId)
                {
                    string sql = "SELECT Id, Name FROM Authors where Id=@Id";

                    SqlCommand command = new SqlCommand(sql, connection);
                    SqlParameter Parameter = new SqlParameter("Id", authoridnew);
                    command.Parameters.Add(Parameter);
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            authors.Add(new Author
                            {
                                Id = Convert.ToInt32(dataReader["Id"]),
                                Name = dataReader["Name"].ToString()
                            });
                        }
                    }
                }

            }

            return authors;
        }

        public Book GetById(int id)
        {
            Book bookToEdit = new Book();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "Select * From Books Where Id = @Id";

                SqlCommand command = new SqlCommand(sql, connection);
                SqlParameter idParameter = new SqlParameter("Id", id);
                command.Parameters.Add(idParameter);

                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        bookToEdit.Id = Convert.ToInt32(dataReader["Id"]);
                        bookToEdit.Title = dataReader["Title"].ToString();
                        //bookToEdit.Author = dataReader["Author"].ToString();
                    }
                }
            }

            return bookToEdit;
        }

        public IList<Book> GetByIdNew(int id)
        {
            //Book bookToEdit = new Book();

            var bookList = new List<Book>();
            var bookData = new List<BookAuthor>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "select b.Id, b.Title, a.Name from Books as b JOIN BookAuthor as ba " +
                    "on b.Id = ba.BookId JOIN Authors AS a ON ba.AuthorId = a.Id where BookID = @Id";

                SqlCommand command = new SqlCommand(sql, connection);
                SqlParameter idParameter = new SqlParameter("Id", id);
                command.Parameters.Add(idParameter);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        bookData.Add(new BookAuthor
                        {
                            BookId = Convert.ToInt32(dataReader["Id"]),
                            BookTitle = dataReader["Title"].ToString(),
                            AuthorName = dataReader["Name"].ToString()
                        });
                    }
                }
            }

            var booksByTitle = bookData.GroupBy(b => b.BookTitle).ToList();

            foreach (var book in booksByTitle)
            {
                var record = new Book();

                foreach (var item in book)
                {
                    record.Id = item.BookId;
                    record.Title = item.BookTitle;
                    record.Author.Add(item.AuthorName);
                }

                bookList.Add(record);
            }

            return bookList;

            //return bookToEdit;
        }

        public void Update(int id, Book book)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql =
                    $"Update Books set Title=@Title Where Id=@Id";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    SqlParameter titleParameter = new SqlParameter("Title", book.Title);
                    //SqlParameter authorParameter = new SqlParameter("Author", book.Author);
                    SqlParameter idParameter = new SqlParameter("Id", id);

                    command.Parameters.Add(titleParameter);
                    //command.Parameters.Add(authorParameter);
                    command.Parameters.Add(idParameter);

                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}