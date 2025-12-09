namespace DepoYonetimSistemi.Models
{
    public class Storage
    {
        public int StorageId { get; set; }
        public string Model { get; set; } = string.Empty;
        public string Manufacturer { get; set; } = string.Empty;
        public string Type { get; set; } = null!;      // SSD, HDD
        public int CapacityGB { get; set; }
        public ICollection<ProductStorage> ProductStorages { get; set; } = new List<ProductStorage>();
    }
}