namespace AssetManagementSystem.Models
{
    public class WarrantyCard
    {
        public int Id { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Provider { get; set; }

        public int AssetId { get; set; } // ForeignKey

        public Asset Asset { get; set; }
    }
}
