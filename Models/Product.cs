namespace DepoYonetimSistemi.Models
{
    //Veri tabanı için Product tablosu
    class Product
    {
        public int ProductId { get; set; } //Primary key
        public string Name { get; set; } = "";
        public string Type { get; set; } = "";
        public int Stock { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}