namespace DepoYonetimSistemi.Models
{
    public class ProductStock
    {
        public int Id { get; set; }

        public int ProductId { get; set; } //Foreign key
        public Product? Product { get; set; }

        public int WarehouseId { get; set; } //Foreign key
        public WareHouse? Warehouse { get; set; }

        public int StockQuantity { get; set; } = 0;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}