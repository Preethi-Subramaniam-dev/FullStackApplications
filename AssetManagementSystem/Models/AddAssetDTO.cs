namespace AssetManagementSystem.Models
{
    public class AddAssetDTO
    {
        public string Name { get; set; }

        public string SerialNumber { get; set; }

        public string? WarrantyName { get; set; }

        public string? EmployeeName { get; set; }
    }
}
