namespace DepoYonetimSistemi.Models
{
    public class Cpu
    {
        public int CpuId { get; set; }
        public string Model { get; set; } = null!;
        public int Cores { get; set; }
        public int Threads { get; set; }
        public double BaseClockGHz { get; set; }
        public double? BoostClockGHz { get; set; }
        public string Manufacturer { get; set; } = null!; // Intel / AMD

        public ICollection<ProductCpu> ProductCpus { get; set; } = new List<ProductCpu>();
    }
}