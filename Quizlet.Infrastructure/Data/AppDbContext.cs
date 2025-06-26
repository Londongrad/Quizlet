using Microsoft.EntityFrameworkCore;
using Quizlet.Domain.Entities;

namespace Quizlet.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Set> Sets => Set<Set>();
        public DbSet<Word> Words => Set<Word>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


        }
    }
}
