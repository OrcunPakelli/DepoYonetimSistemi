namespace DepoYonetimSistemi.Models
{
    //Veri tabanı için Product tablosu
    public class Product
    {
        public int ProductId { get; set; } //Primary key
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int Stock { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}