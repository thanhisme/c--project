using WebApplication1.Validations;

namespace WebApplication1.Dtos
{
    public class FileDtos
    {
        [MaxFileSize]
        [AllowedImageExtensions(new string[] { "jpeg", "png", "jpg" })]
        public IFormFile File { get; set; }
    }
}
