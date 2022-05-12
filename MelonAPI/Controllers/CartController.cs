using MelonAPI.Model;
using MelonAPI.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MelonAPI.Controllers
{
    [Route("/cart")]
    [ApiController]
    public class CartController : ControllerBase
    {

        private readonly ICartRepository cartRepository;

        private readonly IContextRepository contextRepository;

        public CartController(ICartRepository cartRepository,
                                 IContextRepository contextRepository)
        {
            this.cartRepository = cartRepository;
            this.contextRepository = contextRepository;
        }

        [HttpGet("/cart")]
        public List<Product> Get()
        {
            Request.Headers.TryGetValue("token", out var token);

            int userId = contextRepository.LoadCurrentUserId(token.ToString());

            return cartRepository.LoadCartProducts(userId);
        }

        [HttpPost("/cart")]
        public void BuyAllProductsFromCart()
        {
            Request.Headers.TryGetValue("token", out var token);

            int userId = contextRepository.LoadCurrentUserId(token.ToString());

            cartRepository.BuyAllProductsFromCart(userId);
        }

        [HttpPost("/cart/{id}")]
        public void AddProductIntoCart(int id)
        {
            Request.Headers.TryGetValue("token", out var token);

            int userId = contextRepository.LoadCurrentUserId(token.ToString());

            cartRepository.AddProductIntoCart(id, userId);
        }

        [HttpDelete("/cart/{id}")]
        public void DeleteProductFromCart(int id)
        {
            Request.Headers.TryGetValue("token", out var token);

            int userId = contextRepository.LoadCurrentUserId(token.ToString());

            cartRepository.DeleteProductFromCart(id, userId);
        }

    }
}
