using Microsoft.EntityFrameworkCore;
using PhoneBookBackend.Models;

namespace PhoneBookBackend.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options): base(options) 
        {
            
        }

        public DbSet<Person> People { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<County> Counties { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<County>(county =>
            {
                county.HasMany(c => c.Cities)
                      .WithOne(city => city.County)
                      .HasForeignKey(city => city.CountyId);
            });

            modelBuilder.Entity<City>(city =>
            {
                city.HasOne(c => c.County)
                    .WithMany(county => county.Cities)
                    .HasForeignKey(c => c.CountyId);
            });

            modelBuilder.Entity<Person>(person =>
            {
                person.HasOne(p => p.City)
                      .WithMany()
                      .HasForeignKey(p => p.CityId);
            });

            modelBuilder.Entity<Person>()
                .HasIndex(p => p.PhoneNumber)
                .IsUnique();

            modelBuilder.Entity<City>()
                .HasIndex(c => c.Name)
                .IsUnique();

            modelBuilder.Entity<County>()
                .HasIndex(c => c.Name)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }

    }

}
