using Application.Features.Files.Commands.Delete;
using Application.Features.Files.Commands.Upload;

using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController(UploadFileService uploadFileService,
        DeleteFileService deleteFileService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile file
            //, [FromHeader(Name = "X-Idempotency-Key")] string requestId
            )
        {
            //if (!Guid.TryParse(requestId, out var parsedRequestId))
            //{
            //    return BadRequest("Invalid or missing idempotency key.");
            //}

            var cancellationToken = HttpContext.RequestAborted;

            var request = new UploadFileRequest(file.Name, file.OpenReadStream(), file.ContentType);
            var response = await uploadFileService.Execute(request, cancellationToken);

            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteFile([FromBody] DeleteFileRequest request)
        {
            var cancellationToken = HttpContext.RequestAborted;

            await deleteFileService.Execute(request, cancellationToken);

            return Ok();
        }
    }
}
