using System.ComponentModel.DataAnnotations;

namespace ECommerceSolution.DTOs.Product
{
    public class CreateProductDto
    {
        [Required]
        [StringLength(150)]
        public string Name { get; set; }
            = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Range(1, 10000000)]
        public decimal Price { get; set; }

        [Range(0, 100000)]
        public int StockQuantity { get; set; }

        public string? ImageUrl { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}