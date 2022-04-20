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

    }

}
