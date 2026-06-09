namespace ECommerceSolution.DTOs.Product
{
    public class ProductSearchDto
    {
        //product dto comment
        public string? Keyword { get; set; }

        public string? Sort { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 5;
    }
}