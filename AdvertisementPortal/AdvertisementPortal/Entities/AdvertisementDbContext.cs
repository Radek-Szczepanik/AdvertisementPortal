using Microsoft.EntityFrameworkCore;

namespace AdvertisementPortal.Entities
{
    public class AdvertisementDbContext : DbContext
    {
        private string connectionString = "Server=DESKTOP-6607MV5\\SQLEXPRESS;Database=AdvertisementDb;Trusted_Connection=True;";
        public DbSet<Advertisement> Advertisements { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

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

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired();

            modelBuilder.Entity<Role>()
                .Property(r => r.Name)
                .IsRequired();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
