namespace DepoYonetimSistemi.Models
{
    public enum TransactionType
    {
        In,
        Out
    }
    //Veri tabanı için Transaction tablosu
    public class Transaction
    {
        public int TransactionId { get; set; } //Primary Key

        public int? ProductId { get; set; } //Foreign Key
        public Product? Product { get; set; }

        public int UserId { get; set; } //Foreign Key
        public User? User { get; set; }

        public string SeriNo { get; set; } = string.Empty;
        public TransactionType TransactionType { get; set; } = TransactionType.In; //In veya Out, Default "In"
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}