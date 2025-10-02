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

        /*
         Vi valgte at bruge en factory class, så vi kunne undgå at skabe en ny AppDbContext klasse hvergang vi skulle bruge den i ViewModel. 
        Så undgår vi gentagelser, og koden forbliver læsbar.
         */

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