namespace DepoYonetimSistemi.Models
{
    public class ProductScreen
    {
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int ScreenId { get; set; }
        public Screen Screen { get; set; } = null!;
    }
}