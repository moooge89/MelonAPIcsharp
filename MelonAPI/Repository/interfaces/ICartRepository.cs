using MelonAPI.Model;

namespace MelonAPI.Repository {
    public interface ICartRepository
    {
        List<Product> LoadCartProducts(int userId);

        void AddProductIntoCart(int productId, int userId);

        void BuyAllProductsFromCart(int userId);

        void DeleteProductFromCart(int productId, int userId);
    }
}
