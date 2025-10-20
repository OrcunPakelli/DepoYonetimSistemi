namespace DepoYonetimSistemi.Models
{
    public class WareHouse
    {
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<ProductStock> ProductStocks { get; set; } = new List<ProductStock>();
    }
}