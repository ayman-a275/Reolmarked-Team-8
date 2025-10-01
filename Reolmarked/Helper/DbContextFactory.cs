using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Reolmarked.Helper
{
    public static class DbContextFactory
    {
        private static string? _connectionString;

        public static void Initialize(IConfigurationRoot configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public static AppDbContext CreateContext()
        {
            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                throw new InvalidOperationException("Database connection string not initialized.");
            }
            return new AppDbContext(_connectionString);
        }
    }
}