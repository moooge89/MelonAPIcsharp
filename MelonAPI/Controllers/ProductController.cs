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

        public ProductController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        [HttpGet("/product/{id}")]
        public Product Get(int Id)
        {
            return productRepository.LoadProductById(Id, 1);
        }

        [HttpGet("/product/category/{id}")]
        public List<ProductLight> GetByCategoryId(int Id)
        {
            return productRepository.LoadProductByCategoryId(Id, 1);
        }

    }

}
