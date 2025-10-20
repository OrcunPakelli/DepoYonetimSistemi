namespace DepoYonetimSistemi.Models
{
    //Veri tabanı için Product tablosu
    public class Product
    {
        public int ProductId { get; set; } //Primary key
        public string ProductName { get; set; } = string.Empty;

        public int CategoryId { get; set; } //Foreign Key
        public Category? Category { get; set; }

        public int Stock { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public ICollection<ProductStock> ProductStocks { get; set; } = new List<ProductStock>();
    }
}