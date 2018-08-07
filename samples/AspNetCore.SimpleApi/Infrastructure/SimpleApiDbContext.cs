using AspNetCore.SimpleApi.Model;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.SimpleApi.Infrastructure
{
    public class SimpleApiDbContext : DbContext
    {
        public DbSet<Todo> Todos { get; set; }

        public SimpleApiDbContext(DbContextOptions<SimpleApiDbContext> options) : base(options)
        {
            // NOTE: Intentionally left blank.
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Todo>().HasKey(todo => todo.Id);
            modelBuilder.Entity<Todo>().Property(todo => todo.Name).IsRequired();
            modelBuilder.Entity<Todo>().Property(todo => todo.Description).IsRequired(false);
        }
    }
}
