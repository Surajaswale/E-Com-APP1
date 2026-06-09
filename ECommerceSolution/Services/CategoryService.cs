using ECommerceSolution.DTOs.Category;
using ECommerceSolution.Entities;
using ECommerceSolution.Interfaces;

namespace ECommerceSolution.Services
{
    public class CategoryService
        : ICategoryService
    {
        private readonly
            ICategoryRepository _repository;

        public CategoryService(
            ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<CategoryResponseDto>>
            GetAllAsync()
        {
            var categories =
                await _repository.GetAllAsync();

            return categories.Select(c =>
                new CategoryResponseDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    CreatedAt = c.CreatedAt
                }).ToList();
        }

        public async Task<CategoryResponseDto?>
            GetByIdAsync(int id)
        {
            var category =
                await _repository.GetByIdAsync(id);

            if (category == null)
                return null;

            return new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                CreatedAt = category.CreatedAt
            };
        }

        public async Task<CategoryResponseDto>
            CreateAsync(CreateCategoryDto dto)
        {
            var category = new Category
            {
                Name = dto.Name,
                Description = dto.Description
            };

            await _repository.AddAsync(category);

            await _repository.SaveChangesAsync();

            return new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                CreatedAt = category.CreatedAt
            };
        }

        public async Task<bool>
            UpdateAsync(
                int id,
                UpdateCategoryDto dto)
        {
            var category =
                await _repository.GetByIdAsync(id);

            if (category == null)
                return false;

            category.Name = dto.Name;
            category.Description =
                dto.Description;

            _repository.Update(category);

            await _repository.SaveChangesAsync();

            return true;
        }

        public async Task<bool>
            DeleteAsync(int id)
        {
            var category =
                await _repository.GetByIdAsync(id);

            if (category == null)
                return false;

            _repository.Delete(category);

            await _repository.SaveChangesAsync();

            return true;
        }
    }
}