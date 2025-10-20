namespace DepoYonetimSistemi.Models
{
    public class Category
    {
        public int CategoryId { get; set; } //Primary key
        public string CategoryName { get; set; } = string.Empty;

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}