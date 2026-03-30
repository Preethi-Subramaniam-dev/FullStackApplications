using System.ComponentModel.DataAnnotations;

namespace AssetManagementSystem.Models
{
    public class Asset
    {
        [Key]
        public int Id { get; set; }
        public string AssetId { get; set; }
        public string Name { get; set; }
        public string SerialNumber { get; set; }

        public int StatusId { get; set; } // ForeignKey
        public Status Status { get; set; }

        public WarrantyCard WarrantyCard { get; set; }

        public int? EmployeeId { get; set; } // ForeignKey

        public Employee Employee { get; set; }

        public ICollection<AssetSoftware> AssetSoftwares{ get; set; }

    }
}
