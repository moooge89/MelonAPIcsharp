using MelonAPI.Model;
using MelonAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace MelonAPI.Controllers
{
    [Route("/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly IProductRepository productRepository;

        private readonly IContextRepository contextRepository;

        public ProductController(IProductRepository productRepository,
                                 IContextRepository contextRepository)
        {
            this.productRepository = productRepository;
            this.contextRepository = contextRepository;
        }

        [HttpGet("/product/{id}")]
        public Product Get(int Id)
        {
            Request.Headers.TryGetValue("token", out var token);

            int userId = contextRepository.LoadCurrentUserId(token.ToString());

            return productRepository.LoadProductById(Id, userId);
        }

        [HttpGet("/product/of-category/{id}")]
        public List<ProductLight> GetByCategoryId(int id)
        {
            Request.Headers.TryGetValue("token", out var token);

            int userId = contextRepository.LoadCurrentUserId(token.ToString());

            return productRepository.LoadProductByCategoryId(id, userId);
        }


        [HttpGet("/product/filter")]
        public List<ProductLight> LoadProductsWithFilter([FromBody] ProductFilter filter)
        {
            Request.Headers.TryGetValue("token", out var token);

            int userId = contextRepository.LoadCurrentUserId(token.ToString());

            return productRepository.LoadProductWithFilter(filter, userId);
        }

        [HttpPost("/product")]
        public Product Save([FromBody] Product product)
        {
            return productRepository.SaveProduct(product);
        }

        [HttpPut("/product/{id}")]
        public Product Update(int id, [FromBody] Product product)
        {
            return productRepository.UpdateProduct(id, product);
        }

        [HttpDelete("/product/{id}")]
        public void Delete(int id)
        {
            productRepository.DeleteProduct(id);
        }

    }

}
