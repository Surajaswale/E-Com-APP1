using ECommerceSolution.Data;
using ECommerceSolution.Entities;
using ECommerceSolution.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerceSolution.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
        }

        public void Update(Product product)
        {
            _context.Products.Update(product);
        }

        public void Delete(Product product)
        {
            _context.Products.Remove(product);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<List<Product>>
            SearchProductsAsync(
                string? keyword,
                string? sort,
                int page,
                int pageSize)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .AsQueryable();

            // SEARCH

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(p =>
                    p.Name.Contains(keyword));
            }

            // SORT

            query = sort?.ToLower() switch
            {
                "price_asc" =>
                    query.OrderBy(p => p.Price),

                "price_desc" =>
                    query.OrderByDescending(p => p.Price),

                "name_asc" =>
                    query.OrderBy(p => p.Name),

                "name_desc" =>
                    query.OrderByDescending(p => p.Name),

                _ =>
                    query.OrderBy(p => p.Id)
            };

            // PAGINATION

            query = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            return await query.ToListAsync();
        }
    }
}