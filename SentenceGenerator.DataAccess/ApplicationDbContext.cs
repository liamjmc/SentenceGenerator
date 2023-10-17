using Microsoft.EntityFrameworkCore;
using SentenceGenerator.DataAccess.Models;

namespace SentenceGenerator.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Sentence> Sentences { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "GeneratedSentencesDb");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sentence>()
                .HasKey(s => s.Id);
        }
    }
}