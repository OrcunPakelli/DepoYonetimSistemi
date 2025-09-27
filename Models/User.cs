namespace DepoYonetimSistemi.Models
{
    //Veri tabanı için User tablosu
    class User
    {
        public int UserId { get; set; } //Primary key
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";
        public string Role { get; set; } = "";
        public DateTime CreatedAt { get; set; }

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}