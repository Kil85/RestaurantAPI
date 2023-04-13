using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace RestaurantAPI.Controllers
{
    [Route("file")]
    [Authorize]
    public class StaticFilesController : ControllerBase
    {
        [HttpGet]
        [ResponseCache(Duration = 300, VaryByQueryKeys = new[] { "fileName" })]
        public ActionResult GetFile([FromQuery] string fileName)
        {
            var rootPath = Directory.GetCurrentDirectory();
            var filePath = $"{rootPath}/StaticFiles/{fileName}";
            var doesExsist = Path.Exists(filePath);

            if (!doesExsist)
            {
                return NotFound();
            }

            var contentProvider = new FileExtensionContentTypeProvider();
            contentProvider.TryGetContentType(filePath, out var contentType);
            var result = System.IO.File.ReadAllBytes(filePath);

            return File(result, contentType, fileName);
        }

        [HttpPost]
        public ActionResult UploadFile([FromForm] IFormFile file)
        {
            if (file == null || file.Length < 1)
            {
                return BadRequest();
            }

            var rootPath = Directory.GetCurrentDirectory();
            var fileName = file.FileName;
            var filePath = $"{rootPath}/StaticFiles/{fileName}";

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return Ok();
        }
    }
}
