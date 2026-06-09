using ECommerceSolution.Data;
using ECommerceSolution.Entities;
using ECommerceSolution.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerceSolution.Repositories
{
    public class CategoryRepository
        : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(
            AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>>
            GetAllAsync()
        {
            return await _context.Categories
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<Category?>
            GetByIdAsync(int id)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddAsync(
            Category category)
        {
            await _context.Categories
                .AddAsync(category);
        }

        public void Update(
            Category category)
        {
            _context.Categories
                .Update(category);
        }

        public void Delete(
            Category category)
        {
            _context.Categories
                .Remove(category);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}