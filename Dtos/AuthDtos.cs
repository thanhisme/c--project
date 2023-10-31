using System.ComponentModel.DataAnnotations;
using WebApplication1.Constants.Strings;

namespace WebApplication1.Dtos
{
    public class SignUpDto
    {
        [Required(ErrorMessage = ValidationErrorMessages.FIELD_REQUIRED_ERROR_MESSAGE)]
        [StringLength(50, MinimumLength = 5, ErrorMessage = ValidationErrorMessages.LENGTH_RANGE_ERROR_MESSAGE)]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = ValidationErrorMessages.FIELD_REQUIRED_ERROR_MESSAGE)]
        [StringLength(16, MinimumLength = 5, ErrorMessage = ValidationErrorMessages.LENGTH_RANGE_ERROR_MESSAGE)]
        public string Password { get; set; } = string.Empty;
    }

    public class SignInDto
    {
        [Required(ErrorMessage = ValidationErrorMessages.FIELD_REQUIRED_ERROR_MESSAGE)]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = ValidationErrorMessages.FIELD_REQUIRED_ERROR_MESSAGE)]
        public string Password { get; set; } = string.Empty;
    }
}
