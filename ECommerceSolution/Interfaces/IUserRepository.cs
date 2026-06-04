using ECommerceSolution.Entities;

namespace ECommerceSolution.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);

        Task<User?> GetByIdAsync(int id);

        Task AddUserAsync(User user);

        Task SaveChangesAsync();
    }
}