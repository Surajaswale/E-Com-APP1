using ECommerceSolution.DTOs.Category;

namespace ECommerceSolution.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryResponseDto>>
            GetAllAsync();

        Task<CategoryResponseDto?>
            GetByIdAsync(int id);

        Task<CategoryResponseDto>
            CreateAsync(CreateCategoryDto dto);

        Task<bool>
            UpdateAsync(
                int id,
                UpdateCategoryDto dto);

        Task<bool>
            DeleteAsync(int id);
    }
}