using HoundBot.Models;
using Microsoft.EntityFrameworkCore;

namespace HoundBot.Repository
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Quote>  Quotes { get; set; }
        public DbSet<StarboardMessage> StarboardMessages { get; set; }
        private readonly string _connectionString;

        public DatabaseContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite($"Data Source={_connectionString}");
    }
}