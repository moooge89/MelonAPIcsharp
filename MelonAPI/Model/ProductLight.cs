namespace MelonAPI.Model
{
    public class ProductLight
    {
        public int? id { get; set; }

        public string? name { get; set; }
        
        public int? price { get; set; }
        
        public int? categoryId { get; set; }
        
        public bool? isInWishlist { get; set; }
        
        public bool? isInCart { get; set; }

        public byte[]? image { get; set; }
    }
}
