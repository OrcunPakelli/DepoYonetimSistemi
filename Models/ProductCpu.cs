namespace DepoYonetimSistemi.Models
{
    public class ProductCpu
    {
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int CpuId { get; set; }
        public Cpu Cpu { get; set; } = null!;
    }
}