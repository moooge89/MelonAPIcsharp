using MelonAPI.Model;
using MelonAPI.Model.exception;

namespace MelonAPI.Repository.impl
{
    public class ProductRepository : IProductRepository
    {

        private List<Product> products = new List<Product> { };

        public ProductRepository(ICategoryRepository categoryRepository)
        {
            Product product1 = new Product();
            product1.Id = 1;
            product1.Name = "Product Name1";
            product1.Description = "Product Description1";
            product1.Manufacturer = "Product Manufacturer1";
            product1.Price = 10000.0;
            product1.Category = categoryRepository.LoadCategoryById(1);
            products.Add(product1);

            Product product2 = new Product();
            product2.Id = 2;
            product2.Name = "Product Name2";
            product2.Description = "Product Description2";
            product2.Manufacturer = "Product Manufacturer2";
            product2.Price = 20000.0;
            product2.Category = categoryRepository.LoadCategoryById(2);
            products.Add(product2);

            Product product3 = new Product();
            product3.Id = 3;
            product3.Name = "Product Name3";
            product3.Description = "Product Description3";
            product3.Manufacturer = "Product Manufacturer3";
            product3.Price = 30000.0;
            product3.Category = categoryRepository.LoadCategoryById(3);
            products.Add(product3);
        }
        
        public List<Product> LoadProductByCategoryId(int CategoryId)
        {
            return products.Where(p => p.Category?.Id == CategoryId).ToList();
        }

        public Product LoadProductById(int Id)
        {
            var product = products.Where(p => p.Id == Id).FirstOrDefault();

            if (product == null)
            {
                throw new IdNotFoundException($"Product with id {Id} was not found");
            }

            return product;
        }
    }
}
