namespace DepoYonetimSistemi.Models
{
    public class ProductRam
    {
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int RamId { get; set; }
        public Ram Ram { get; set; } = null!;
    }
}