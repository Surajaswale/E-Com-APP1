using ECommerceSolution.DTOs.Product;
using ECommerceSolution.Entities;
using ECommerceSolution.Interfaces;

namespace ECommerceSolution.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductService(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<List<ProductResponseDto>> GetAllAsync()
        {
            var products =
                await _productRepository.GetAllAsync();

            return products.Select(p =>
                new ProductResponseDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    ImageUrl = p.ImageUrl,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category?.Name ?? "",
                    CreatedAt = p.CreatedAt
                }).ToList();
        }

        public async Task<ProductResponseDto?> GetByIdAsync(int id)
        {
            var product =
                await _productRepository.GetByIdAsync(id);

            if (product == null)
                return null;

            return new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                ImageUrl = product.ImageUrl,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name ?? "",
                CreatedAt = product.CreatedAt
            };
        }

        public async Task<ProductResponseDto> CreateAsync(
            CreateProductDto dto)
        {
            var category =
                await _categoryRepository.GetByIdAsync(
                    dto.CategoryId);

            if (category == null)
            {
                throw new Exception(
                    "Category not found.");
            }

            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                StockQuantity = dto.StockQuantity,
                ImageUrl = dto.ImageUrl,
                CategoryId = dto.CategoryId
            };

            await _productRepository.AddAsync(product);

            await _productRepository.SaveChangesAsync();

            return new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                ImageUrl = product.ImageUrl,
                CategoryId = product.CategoryId,
                CategoryName = category.Name,
                CreatedAt = product.CreatedAt
            };
        }

        public async Task<bool> UpdateAsync(
            int id,
            UpdateProductDto dto)
        {
            var product =
                await _productRepository.GetByIdAsync(id);

            if (product == null)
                return false;

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.StockQuantity = dto.StockQuantity;
            product.ImageUrl = dto.ImageUrl;
            product.CategoryId = dto.CategoryId;

            _productRepository.Update(product);

            await _productRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product =
                await _productRepository.GetByIdAsync(id);

            if (product == null)
                return false;

            _productRepository.Delete(product);

            await _productRepository.SaveChangesAsync();

            return true;
        }

        // ==========================================
        // SEARCH + SORTING + PAGINATION
        // ==========================================

        public async Task<List<ProductResponseDto>>
            SearchProductsAsync(
                ProductSearchDto dto)
        {
            var products =
                await _productRepository
                .SearchProductsAsync(
                    dto.Keyword,
                    dto.Sort,
                    dto.Page,
                    dto.PageSize);

            return products
                .Select(p =>
                    new ProductResponseDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        Price = p.Price,
                        StockQuantity = p.StockQuantity,
                        ImageUrl = p.ImageUrl,
                        CategoryId = p.CategoryId,
                        CategoryName = p.Category?.Name ?? "",
                        CreatedAt = p.CreatedAt
                    })
                .ToList();
        }
    }
}