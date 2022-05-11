using MelonAPI.Model;
using MelonAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace MelonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {

        private readonly IWishlistRepository wishlistRepository;

        private readonly IContextRepository contextRepository;

        public WishlistController(IWishlistRepository wishlistRepository,
                                 IContextRepository contextRepository)
        {
            this.wishlistRepository = wishlistRepository;
            this.contextRepository = contextRepository;
        }

        [HttpGet("/wishlist")]
        public List<Product> Get()
        {
            Request.Headers.TryGetValue("token", out var token);

            int userId = contextRepository.LoadCurrentUserId(token.ToString());

            return wishlistRepository.LoadWishlistProducts(userId);
        }


        [HttpPost("/wishlist/{id}")]
        public void AddProductIntoWishlist(int id)
        {
            Request.Headers.TryGetValue("token", out var token);

            int userId = contextRepository.LoadCurrentUserId(token.ToString());

            wishlistRepository.AddProductIntoWishlist(id, userId);
        }

        [HttpDelete("/wishlist/{id}")]
        public void DeleteProductFromWishlist(int id)
        {
            Request.Headers.TryGetValue("token", out var token);

            int userId = contextRepository.LoadCurrentUserId(token.ToString());

            wishlistRepository.DeleteProductFromWishlist(id, userId);
        }

    }
}
