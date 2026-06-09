using ECommerceSolution.DTOs.Product;

namespace ECommerceSolution.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductResponseDto>>
            GetAllAsync();

        Task<ProductResponseDto?>
            GetByIdAsync(int id);

        Task<ProductResponseDto>
            CreateAsync(CreateProductDto dto);

        Task<bool>
            UpdateAsync(
                int id,
                UpdateProductDto dto);

        Task<bool>
            DeleteAsync(int id);

        // Search + Sorting + Pagination
        Task<List<ProductResponseDto>>
            SearchProductsAsync(
                ProductSearchDto dto);
    }
}