using MelonAPI.Model;
using MelonAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace MelonAPI.Controllers
{
    [Route("/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {

        private readonly ICategoryRepository categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        [HttpGet]
        public List<Category> Get()
        {
           return categoryRepository.LoadAllCategories();
        }

        [HttpGet("/category/{id}")]
        public Category GetById(int id)
        {
            return categoryRepository.LoadCategoryById(id);
        }

        [HttpPost("/category")]
        public Category Save([FromBody] Category category)
        {
            return categoryRepository.SaveCategory(category);
        }

        [HttpPut("/category/{id}")]
        public Category Update(int id, [FromBody] Category category)
        {
            return categoryRepository.UpdateCategory(id, category);
        }

        [HttpDelete("/category/{id}")]
        public void Delete(int id)
        {
            categoryRepository.DeleteCategoryById(id);
        }

    }

}
