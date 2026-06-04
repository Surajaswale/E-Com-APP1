using ECommerceSolution.Data;
using ECommerceSolution.Entities;
using ECommerceSolution.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerceSolution.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(
            AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByEmailAsync(
            string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(
                    x => x.Email == email);
        }

        public async Task<User?> GetByIdAsync(
            int id)
        {
            return await _context.Users
                .FindAsync(id);
        }

        public async Task AddUserAsync(
            User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}