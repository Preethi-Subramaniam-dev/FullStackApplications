using AssetManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace AssetManagementSystem.Data
{
    public class AssetManagementDBContext : DbContext
    {
        public AssetManagementDBContext(DbContextOptions<AssetManagementDBContext> options) : base(options)
        {

        }

        public DbSet<Asset> Assets { get; set; }
        public DbSet<WarrantyCard> WarrantyCards { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<SoftwareLicense> SoftwareLicenses { get; set; }

        public DbSet<AssetSoftware> AssetSoftwares { get; set; }

        public DbSet<Status> Statuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Assets Table Configuration
            modelBuilder.ApplyConfiguration(new Config.AssetConfig());

            modelBuilder.Entity<Asset>()
                .HasOne(a => a.Status)
                .WithMany(s => s.Assets)
                .HasForeignKey(a => a.StatusId);

            modelBuilder.Entity<Asset>()
               .HasOne(a => a.WarrantyCard)
               .WithOne(w => w.Asset)
               .HasForeignKey<WarrantyCard>(w => w.Id)
               .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<WarrantyCard>()
            //    .HasKey(w => w.Id);


            modelBuilder.Entity<AssetSoftware>()
                .HasKey(asset => new {asset.AssetId, asset.SoftwareLicenseId});
            modelBuilder.Entity<AssetSoftware>()
                .HasOne(asset => asset.Asset)
                .WithMany(asset => asset.AssetSoftwares)
                .HasForeignKey(asset => asset.AssetId);

            modelBuilder.Entity<AssetSoftware>()
                .HasOne(asset => asset.SoftwareLicense)
                .WithMany(software => software.AssetSoftwares)
                .HasForeignKey(asset => asset.SoftwareLicenseId);

            modelBuilder.Entity<Status>().HasData(
                new Status { StatusId = 1, StatusName = "Available" },
                new Status { StatusId = 2, StatusName = "Assigned" },
                new Status { StatusId = 3, StatusName = "Retired" }
            );
        }
    }
}
