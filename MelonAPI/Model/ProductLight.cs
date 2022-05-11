namespace MelonAPI.Model
{
    public class ProductLight
    {
        public int Id { get; set; }

        public string? Name { get; set; }
        
        public decimal Price { get; set; }
        
        public int CategoryId { get; set; }
        
        public bool IsInWishlist { get; set; }
        
        public bool IsInCart { get; set; }
    }
}
