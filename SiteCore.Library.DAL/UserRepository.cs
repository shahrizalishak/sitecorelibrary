using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SiteCore.Library.BAL.Entities;
using SiteCore.Library.BAL.Interfaces;

namespace SiteCore.Library.DAL
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _configuration;
        readonly string connectionString;
        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration["ConnectionStrings:DefaultConnection"];
        }

        public void Create(User user)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = $"Insert Into Users (Name, Email, MobileNo) Values " +
                    $"(@Name,@Email,@MobileNo)";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    SqlParameter nameParameter = new SqlParameter("Name", user.Name);
                    SqlParameter emailParameter = new SqlParameter("Email", user.Email);
                    SqlParameter mobileParameter = new SqlParameter("MobileNo", user.MobileNo);

                    command.Parameters.Add(nameParameter);
                    command.Parameters.Add(emailParameter);
                    command.Parameters.Add(mobileParameter);

                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int userId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql =
                    $"Delete From Users Where Id=@Id";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    SqlParameter idParameter = new SqlParameter("Id", userId);

                    command.Parameters.Add(idParameter);

                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
            }
        }

        public IList<User> GetAll()
        {
            var userList = new List<User>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "Select * From Users";

                SqlCommand command = new SqlCommand(sql, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        userList.Add(new User
                        {
                            Id = Convert.ToInt32(dataReader["Id"]),
                            Name = dataReader["Name"].ToString(),
                            Email = dataReader["Email"].ToString(),
                            MobileNo = dataReader["MobileNo"].ToString()
                    });
                    }
                }
            }

            return userList;
        }

        public User GetById(int id)
        {
            User userToEdit = new User();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "Select * From Users Where Id = @Id";

                SqlCommand command = new SqlCommand(sql, connection);
                SqlParameter idParameter = new SqlParameter("Id", id);
                command.Parameters.Add(idParameter);

                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        userToEdit.Id = Convert.ToInt32(dataReader["Id"]);
                        userToEdit.Name = dataReader["Name"].ToString();
                        userToEdit.Email = dataReader["Email"].ToString();
                        userToEdit.MobileNo = dataReader["MobileNo"].ToString();
                    }
                }
            }

            return userToEdit;
        }

        public void Update(int id, User user)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql =
                    $"Update Users set Name=@Name, Email=@Email, MobileNo=@MobileNo Where Id=@Id";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    SqlParameter nameParameter = new SqlParameter("Name", user.Name);
                    SqlParameter emailParameter = new SqlParameter("Email", user.Email);
                    SqlParameter mobileParameter = new SqlParameter("MobileNo", user.MobileNo);
                    SqlParameter idParameter = new SqlParameter("Id", user.Id);

                    command.Parameters.Add(nameParameter);
                    command.Parameters.Add(emailParameter);
                    command.Parameters.Add(mobileParameter);
                    command.Parameters.Add(idParameter);

                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
            }
        }

    }
}
