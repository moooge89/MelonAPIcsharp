using MelonAPI.Model;

namespace MelonAPI.Repository.impl
{
    public interface ICategoryRepository
    {
        List<Category> LoadAllCategories();

        Category LoadCategoryById(int Id);

        Category SaveCategory(Category category);
    }
}
