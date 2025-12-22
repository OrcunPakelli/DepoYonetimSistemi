namespace DepoYonetimSistemi.Models
{
    public class ProductStock
    {
        public int ProductId { get; set; } //Foreign key
        public Product? Product { get; set; }

        public int WarehouseId { get; set; } //Foreign key
        public WareHouse? Warehouse { get; set; }

        public string SeriNumber { get; set; } = string.Empty;
    }
}