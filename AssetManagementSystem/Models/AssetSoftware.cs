namespace AssetManagementSystem.Models
{
    public class AssetSoftware
    {
        public int AssetId { get; set; }

        public Asset Asset { get; set; }

        public int SoftwareLicenseId { get; set; }

        public SoftwareLicense SoftwareLicense { get; set; }
    }
}
