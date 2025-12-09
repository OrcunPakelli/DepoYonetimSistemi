namespace DepoYonetimSistemi.Models
{
    public class Ram
    {
        public int RamId { get; set; }
        public string Model { get; set; } = string.Empty;
        public string Manufacturer { get; set; } = string.Empty;
        public string Type { get; set; } = null!;  // DDR4, DDR5
        public int SizeGB { get; set; }
        public int? SpeedMHz { get; set; }
        public ICollection<ProductRam> ProductRams { get; set; } = new List<ProductRam>();
    }
}