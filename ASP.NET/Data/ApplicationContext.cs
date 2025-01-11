using Microsoft.EntityFrameworkCore;
using ASP.NET.Models;

namespace ASP.NET.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        public DbSet<Superhero> Superheroes { get; set; }
        


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Superhero>().ToTable("superhero");
        }
    }
}