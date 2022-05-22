namespace MelonAPI.Model
{
    public class Category
    {
        public int? id { get; set; }
        public string? name { get; set; }

        public byte[]? icon { get; set; }

        public int? productCount { get; set; }
        
        public Category() { }

    
    }
}
