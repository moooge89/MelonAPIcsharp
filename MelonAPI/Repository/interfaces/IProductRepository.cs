using MelonAPI.Model;

namespace MelonAPI.Repository
{
    public interface IProductRepository
    {
        Product LoadProductById(int productId, int userId);

        List<ProductLight> LoadProductByCategoryId(int categoryId, int userId);
    }
}
