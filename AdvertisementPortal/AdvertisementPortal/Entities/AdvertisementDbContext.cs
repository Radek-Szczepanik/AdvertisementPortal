using Microsoft.EntityFrameworkCore;

namespace AdvertisementPortal.Entities
{
    public class AdvertisementDbContext : DbContext
    {
        private string connectionString = "Server=DESKTOP-6607MV5\\SQLEXPRESS;Database=AdvertisementDb;Trusted_Connection=True;";
        public DbSet<Advertisement> Advertisements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Advertisement>()
                .Property(a => a.Title)
                .IsRequired()
                .HasMaxLength(30);

            modelBuilder.Entity<Advertisement>()
                .Property(a => a.Content)
                .IsRequired()
                .HasMaxLength(1000);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
