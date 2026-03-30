using AssetManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace AssetManagementSystem.Data.Config
{
    public class AssetConfig : Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<Asset>
    {
        public void Configure(EntityTypeBuilder<Asset> builder)
        {
            builder.ToTable("Assets");
            builder.HasKey(x => x.Id);

            builder.Property(a=>a.Id).UseIdentityColumn();
            builder.Property(a=>a.AssetId).HasComputedColumnSql("'SOFTHARD-' + CAST(Id AS VARCHAR)");
            builder.Property(a => a.Name).IsRequired().HasMaxLength(100);
            builder.Property(a=> a.SerialNumber).IsRequired().HasMaxLength(50);

            builder.HasData(
                new Asset { Id = 1, Name = "Laptop", SerialNumber = "SN123456", StatusId = 1 },
                new Asset { Id = 2, Name = "Desktop", SerialNumber = "SN654321", StatusId = 2 },
                new Asset { Id = 3, Name = "Printer", SerialNumber = "SN789012", StatusId = 3 }
            );
        }
    }
}
