namespace DepoYonetimSistemi.Models
{
    //Veri tabanı için Transaction tablosu
    public class Transaction
    {
        public int TransactionId { get; set; } //Primary Key

        public int ProductId { get; set; } //Foreign Key
        public Product? Product { get; set; }

        public int UserId { get; set; } //Foreign Key
        public User? User { get; set; }

        public int Quantity { get; set; }
        public string TransactionType { get; set; } = "IN"; //In veya Out, Default "In"
        public DateTime CreatedAt { get; set; }
    }
}