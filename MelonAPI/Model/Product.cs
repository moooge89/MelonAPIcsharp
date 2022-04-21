namespace MelonAPI.Model
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        
        public Double Price { get; set; }

        public Category? Category { get; set; }

        public string? Manufacturer { get; set; }

        public string? Description { get; set; }
    }
}
