namespace MelonAPI.Model
{
    public class Product
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public int Count { get; set; }

        public Category? Category { get; set; }

        public string? Manufacturer { get; set; }

        public bool IsInWishlist { get; set; }
        
        public bool IsInCart { get; set; }

        public int? ImageId { get; set; }

        public int? CategoryId { get; set; }

    }
}
