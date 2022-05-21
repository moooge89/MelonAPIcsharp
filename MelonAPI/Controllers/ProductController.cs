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

        [HttpGet("/product/category/{id}")]
        public List<ProductLight> GetByCategoryId(int Id)
        {
            Request.Headers.TryGetValue("token", out var token);

            int userId = contextRepository.LoadCurrentUserId(token.ToString());

            return productRepository.LoadProductByCategoryId(Id, userId);
        }

        [HttpPost("/product")]
        public Product Save([FromBody] Product product)
        {
            return productRepository.SaveProduct(product);
        }

    }

}
