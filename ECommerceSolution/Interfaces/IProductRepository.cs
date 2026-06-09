using ECommerceSolution.Entities;

namespace ECommerceSolution.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();

        Task<Product?> GetByIdAsync(int id);

        Task AddAsync(Product product);

        void Update(Product product);

        void Delete(Product product);

        Task SaveChangesAsync();

        Task<List<Product>>
            SearchProductsAsync(
                string? keyword,
                string? sort,
                int page,
                int pageSize);
    }
}