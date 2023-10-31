using Microsoft.AspNetCore.Mvc;
using WebApplication1.Constants.Enums;
using WebApplication1.Dtos;
using WebApplication1.Interfaces.Repositories;
using WebApplication1.Utils.HttpExceptionResponse;
using WebApplication1.Utils.HttpResponse;

namespace WebApplication1.Controllers
{
    [Route("[controller]s")]
    [ApiController]
    public class FileController : BaseController
    {
        private readonly IFileService _fileService;
        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost("upload")]
        public async Task<ActionResult<HttpResponse<string>>> Upload([FromForm] FileDtos file)
        {
            var uri = await _fileService.Upload(file.File);

            return SuccessResponse(uri);
        }

        [HttpGet("{filename}")]
        public async Task<FileContentResult> GetByName(string filename)
        {
            var fileContent = await _fileService.GetByName(filename);

            return fileContent == null
                ? throw new HttpExceptionResponse(HttpExceptionTypes.BadRequest, "File not found")
                : File(fileContent, $"image/{Path.GetExtension(filename)[1..]}");
        }

        [HttpGet("{filename}/download")]
        public async Task<FileContentResult> Download(string filename)
        {
            var fileContent = await _fileService.GetByName(filename);

            return fileContent == null
                ? throw new HttpExceptionResponse(HttpExceptionTypes.BadRequest, "File not found")
                : File(fileContent, $"image/{Path.GetExtension(filename)[1..]}", filename);
        }

        [HttpDelete("{filename}/remove")]
        public async Task<ActionResult<HttpResponse<string>>> Remove(string filename)
        {
            var result = await _fileService.Remove(filename);

            return result == null
                ? throw new HttpExceptionResponse(HttpExceptionTypes.BadRequest, "File not found")
                : SuccessResponse(result);
        }
    }
}
