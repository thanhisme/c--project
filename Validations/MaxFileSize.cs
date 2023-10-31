using System.ComponentModel.DataAnnotations;
using WebApplication1.Constants.Numbers;

namespace WebApplication1.Validations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class MaxFileSize : ValidationAttribute
    {
        private int MaxSize { get; }

        public MaxFileSize(int maxSizeInMb = NumberConstants.MAXIMUM_IMAGE_SIZE)
        {
            MaxSize = maxSizeInMb;
        }

        public override bool IsValid(object? value)
        {
            if (value is IFormFile file)
            {
                if (file.Length > 0 && file.Length <= MaxSize * 1024 * 1024)
                {
                    return true;
                }
            }

            return false;
        }
        public override string FormatErrorMessage(string _)
        {
            return $"File size must be less than {MaxSize} MB";
        }
    }
}
