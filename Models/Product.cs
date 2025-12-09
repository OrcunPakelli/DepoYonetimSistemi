namespace DepoYonetimSistemi.Models
{
    //Veri tabanı için Product tablosu
    public class Product
    {
        public int ProductId { get; set; } //Primary key
        public string SerialNumber { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int? LocationId { get; set; }
        public Location? Location { get; set; }

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public ICollection<ProductStock> ProductStocks { get; set; } = new List<ProductStock>();
        public ICollection<ProductCpu> ProductCpus { get; set; } = new List<ProductCpu>();
        public ICollection<ProductGpu> ProductGpus { get; set; } = new List<ProductGpu>();
        public ICollection<ProductRam> ProductRams { get; set; } = new List<ProductRam>();
        public ICollection<ProductStorage> ProductStorages { get; set; } = new List<ProductStorage>();
        public ICollection<ProductScreen> ProductScreens { get; set; } = new List<ProductScreen>();
    }
}