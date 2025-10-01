using Microsoft.EntityFrameworkCore;
using Reolmarked.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reolmarked.Helper
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Product { get; set; }
        public DbSet<Rack> Rack { get; set; }
        public DbSet<RackType> RackType { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<TransactionLine> TransactionLine { get; set; }
        public DbSet<PaymentMethod> PaymentMethod { get; set; }
        public DbSet<Renter> Renter { get; set; }
        public DbSet<RentedRack> RentedRack { get; set; }
        public DbSet<MonthlySettlement> MonthlySettlement { get; set; }

        private readonly string _connectionString;

        public AppDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
