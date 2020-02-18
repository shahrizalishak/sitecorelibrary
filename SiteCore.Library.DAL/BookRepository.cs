using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SiteCore.Library.BAL.Entities;
using SiteCore.Library.BAL.Interfaces;

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

                string sql = $"Insert Into Books (Title, Author) Values (@Title,@Author)";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    SqlParameter titleParameter = new SqlParameter("Title", book.Title);
                    SqlParameter authorParameter = new SqlParameter("Author", book.Author);

                    command.Parameters.Add(titleParameter);
                    command.Parameters.Add(authorParameter);

                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
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

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "Select * From Books";

                SqlCommand command = new SqlCommand(sql, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        bookList.Add(new Book
                        {
                            Id = Convert.ToInt32(dataReader["Id"]),
                            Title = dataReader["Title"].ToString(),
                            Author = dataReader["Author"].ToString()
                        });
                    }
                }
            }

            return bookList;
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
                        bookToEdit.Author = dataReader["Author"].ToString();
                    }
                }
            }

            return bookToEdit;
        }

        public void Update(int id, Book book)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql =
                    $"Update Books set Title=@Title, Author=@Author Where Id=@Id";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    SqlParameter titleParameter = new SqlParameter("Title", book.Title);
                    SqlParameter authorParameter = new SqlParameter("Author", book.Author);
                    SqlParameter idParameter = new SqlParameter("Id", id);

                    command.Parameters.Add(titleParameter);
                    command.Parameters.Add(authorParameter);
                    command.Parameters.Add(idParameter);

                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}