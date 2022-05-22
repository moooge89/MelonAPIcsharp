using MelonAPI.Model;

namespace MelonAPI.Repository
{
    public interface ICategoryRepository
    {
        List<Category> LoadAllCategories();

        Category LoadCategoryById(int Id);

        Category SaveCategory(Category category);

        Category UpdateCategory(int id, Category category);

        void DeleteCategoryById(int Id);
    }
}
