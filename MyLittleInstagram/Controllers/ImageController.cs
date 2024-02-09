using Microsoft.AspNetCore.Mvc;

namespace MyLittleInstagram.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> UploadImage()
        {
            var file = Request.Form.Files[0];

            return Ok();
        }
    }
}