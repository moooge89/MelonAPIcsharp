using MelonAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace MelonAPI.Controllers
{
    [Route("/image")]
    [ApiController]
    public class ImageController : ControllerBase
    {

        private readonly IImageRepository imageRepository;

        public ImageController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        [HttpGet("/image/{id}")]
        public IActionResult Get(int id)
        {
            var fileStream = imageRepository.DownloadImage(id);
            return File(fileStream, "image/jpg");
        }

        [HttpPost("/image"), DisableRequestSizeLimit]
        public void Post([FromForm] IFormFile file)
        {
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();
                imageRepository.UploadImage(fileBytes);
                
            }
        }

    }
}
