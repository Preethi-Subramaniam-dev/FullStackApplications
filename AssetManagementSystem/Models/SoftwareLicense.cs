namespace AssetManagementSystem.Models
{
    public class SoftwareLicense
    {
        public int SoftwareLicenseId { get; set; }

        public string SoftwareName { get; set; }

        public string LicenseKey { get; set; }

        public ICollection<AssetSoftware> AssetSoftwares { get; set; }

    }
}
