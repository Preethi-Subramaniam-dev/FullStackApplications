namespace AssetManagementSystem.Models
{
    public class Status
    {
        public int StatusId { get; set; }

        public string StatusName { get; set; }

        public ICollection<Asset> Assets { get; set; }

    }
}
