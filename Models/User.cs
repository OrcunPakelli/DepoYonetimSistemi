namespace DepoYonetimSistemi.Models
{
    public enum UserRole
    {
        SystemAdmin,
        Manager,
        Employee
    }
    //Veri tabanı için User tablosu
    public class User
    {
        public int UserId { get; set; } //Primary key
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.Employee; //default değer employee
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}