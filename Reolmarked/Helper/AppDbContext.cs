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

        /*
         Vi var allerede startet med og committed i at at bruge Entity Framework, 
         før vi vidste at vi skulle bruge SQL prepared statements.

         Det er vi selvfølelig opmærksomme omkring til eksamensprojektet.
         */

        /*
         AppDbContext arver fra DbContext i Entity Framework og bruges til at udføre queries 
         ved hjælp af objekter i stedet for at skrive SQL direkte, som kan være en sikkerhedstrussel.
         */

        public DbSet<Product> Product { get; set; }
        public DbSet<Shelf> Shelf { get; set; }
        public DbSet<ShelfType> ShelfType { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<TransactionLine> TransactionLine { get; set; }
        public DbSet<PaymentMethod> PaymentMethod { get; set; }
        public DbSet<Renter> Renter { get; set; }
        public DbSet<RentalAgreement> RentalAgreement { get; set; }
        public DbSet<AgreementLine> AgreementLine { get; set; }
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
