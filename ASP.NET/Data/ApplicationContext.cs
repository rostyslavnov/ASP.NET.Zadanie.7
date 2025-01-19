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
        public DbSet<Superpower> Superpowers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Superhero>().ToTable("superhero");
            modelBuilder.Entity<Superpower>().ToTable("superpower");
            modelBuilder.Entity<Superhero>()
                .HasMany(s => s.Superpowers)
                .WithMany(sp => sp.Superheroes)
                .UsingEntity(j => j.ToTable("SuperheroSuperpowers"));
        }
    }
}