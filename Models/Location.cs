namespace DepoYonetimSistemi.Models
{
    public class Location
    {
         public int LocationId { get; set; }
        public string Aisle { get; set; } = string.Empty; // koridor
        public string Shelf { get; set; } = string.Empty; // raf
        public string Bin { get; set; } = string.Empty;   // kutu / bölme
    
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}