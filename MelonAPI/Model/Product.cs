namespace MelonAPI.Model
{
    public class Product
    {
        public int? id { get; set; }

        public string? name { get; set; }

        public string? description { get; set; }

        public decimal? price { get; set; }

        public int? count { get; set; }

        public Category? category { get; set; }

        public string? manufacturer { get; set; }

        public bool? isInWishlist { get; set; }
        
        public bool? isInCart { get; set; }

        public byte[]? image { get; set; }

    }
}
