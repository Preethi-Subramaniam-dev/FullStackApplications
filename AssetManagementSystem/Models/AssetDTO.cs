using System.ComponentModel.DataAnnotations;

namespace AssetManagementSystem.Models
{
    public class AssetDTO
    {
        public string AssetId { get; set; }
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public string? WarrantyName { get; set; }
        public string? EmployeeName { get; set; }

        public List<SoftwareLicenseDTO> SoftwareLicenses { get; set; }
    }

    public class SoftwareLicenseDTO
    {
        public string LicenseName { get; set; }
    }
}
