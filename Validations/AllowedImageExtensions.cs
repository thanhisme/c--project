using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Validations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class AllowedImageExtensions : ValidationAttribute
    {
        private string[] AllowedExtensions { get; }

        public AllowedImageExtensions(string[] allowedExtensions)
        {
            AllowedExtensions = allowedExtensions;
        }

        public override bool IsValid(object? value)
        {
            if (value is IFormFile file)
            {
                var fileExtension = Path.GetExtension(file.FileName).ToLower()[1..];
                Console.WriteLine(fileExtension);
                return AllowedExtensions.Any(ext => ext.ToLower() == fileExtension);
            }

            return false;
        }
        public override string FormatErrorMessage(string _)
        {
            return $"File extension must be in: {AllowedExtensions.Aggregate((ext1, ext2) => $"{ext1}, {ext2}")}";
        }
    }
}
