namespace DepoYonetimSistemi.Models
{
    public class ProductStorage
    {
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int StorageId { get; set; }
        public Storage Storage { get; set; } = null!;
    }
}