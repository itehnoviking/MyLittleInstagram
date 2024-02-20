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
            var ip = GetIpAddress();

            var file = Request.Form.Files[0];

            return Ok();
        }

        private string GetIpAddress()
        {
            if (Request.Headers.ContainsKey("X-forwarded-For"))
            {
                return Request.Headers["X-forwarded-For"];
            }
            else
            {
                return HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
            }
        }
    }
}