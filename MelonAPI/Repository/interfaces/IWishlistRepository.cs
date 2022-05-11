using MelonAPI.Model;

namespace MelonAPI.Repository
{
    public interface IWishlistRepository
    {
        List<Product> LoadWishlistProducts(int userId);

        void AddProductIntoWishlist(int productId, int userId);

        void DeleteProductFromWishlist(int productId, int userId);
    }
}
