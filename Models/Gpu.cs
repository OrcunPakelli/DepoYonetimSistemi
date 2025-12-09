namespace DepoYonetimSistemi.Models
{
    public class Gpu
    {
        public int GpuId { get; set; }
        public string Model { get; set; } = null!;
        public string Memory { get; set; } = null!;
        public string Manufacturer { get; set; } = null!;
        public ICollection<ProductGpu> ProductGpus { get; set; } = new List<ProductGpu>();
    }
}