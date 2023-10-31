using System.ComponentModel.DataAnnotations;
using WebApplication1.Constants.Numbers;

namespace WebApplication1.Dtos
{
    public class PaginationDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than 0")]
        public int Page { get; set; } = NumberConstants.PAGE_DEFAULT;

        [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than 0")]
        public int PageSize { get; set; } = NumberConstants.PAGE_SIZE_DEFAULT;
    }

    public class FilterByKeywordDto : PaginationDto
    {
        public string Keyword { get; set; } = string.Empty;
    }
}
