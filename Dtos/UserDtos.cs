using System.ComponentModel.DataAnnotations;
using WebApplication1.Constants.Strings;

namespace WebApplication1.Dtos
{
    public class UserUpdateDto
    {
        [StringLength(50, MinimumLength = 5, ErrorMessage = ValidationErrorMessages.LENGTH_RANGE_ERROR_MESSAGE)]
        public string? Username { get; set; }

        [StringLength(16, MinimumLength = 5, ErrorMessage = ValidationErrorMessages.LENGTH_RANGE_ERROR_MESSAGE)]
        public string? Password { get; set; }
    }
}
