using MelonAPI.Model;

namespace MelonAPI.Repository
{
    public interface IProductRepository
    {
        Product LoadProductById(int id);

        List<Product> LoadProductByCategoryId(int categoryId);
    }
}
