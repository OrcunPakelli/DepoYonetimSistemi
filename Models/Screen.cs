namespace DepoYonetimSistemi.Models
{
    public class Screen
    {
        public int ScreenId { get; set; }
        public double SizeInches { get; set; }
        public string Resolution { get; set; } = null!;
        public int? RefreshRate { get; set; }
        public string? PanelType { get; set; }
        public ICollection<ProductScreen> ProductScreens { get; set; } = new List<ProductScreen>();
    }
}