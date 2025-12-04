using Application.Features.Files.Commands.Upload;

using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController(UploadFileService uploadFileService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
        {
            var cancellationToken = HttpContext.RequestAborted;

            var request = new UploadFileRequest(file.Name, file.OpenReadStream(), file.ContentType);
            var response = await uploadFileService.Execute(request, cancellationToken);

            return Ok(response);
        }
    }
}
