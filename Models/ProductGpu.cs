namespace DepoYonetimSistemi.Models
{
    public class ProductGpu
    {
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int GpuId { get; set; }
        public Gpu Gpu { get; set; } = null!;
    }
}