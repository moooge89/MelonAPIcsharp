using MelonAPI.Model;
using MelonAPI.Model.exception;

namespace MelonAPI.Repository.impl
{
    public class CategoryRepository : ICategoryRepository
    {

        private List<Category> categories = new List<Category> { 
            new Category(1, "Periphery"),    
            new Category(2, "Smartphone"),
            new Category(3, "Others")
        };

        Category ICategoryRepository.LoadCategoryById(int Id)
        {

            var category = categories.Where(c => c.Id == Id).FirstOrDefault();

            if (category == null)
            {
                throw new IdNotFoundException($"Category with id {Id} was not found");
            }

            return category;
        }

        List<Category> ICategoryRepository.LoadAllCategories()
        {
            return categories;
        }
    }
}
