using System;
using Microsoft.Extensions.Configuration;

namespace SiteCore.Library.DAL
{
    public class UserRepository
    {
        private readonly IConfiguration _configuration;
        readonly string connectionString;
        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration["ConnectionStrings:DefaultConnection"];
        }
    }
}
