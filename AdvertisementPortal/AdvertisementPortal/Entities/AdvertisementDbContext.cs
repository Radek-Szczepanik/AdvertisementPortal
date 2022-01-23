using Microsoft.EntityFrameworkCore;

namespace AdvertisementPortal.Entities
{
    public class AdvertisementDbContext : DbContext
    {
        public AdvertisementDbContext(DbContextOptions<AdvertisementDbContext> options) : base(options) { }
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
    }
}
