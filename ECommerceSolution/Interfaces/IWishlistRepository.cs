using ECommerceSolution.Entities;

namespace ECommerceSolution.Interfaces
{
    public interface IWishlistRepository
    {
        Task<Wishlist?> GetWishlistItemAsync(
            int userId,
            int productId);

        Task<List<Wishlist>> GetUserWishlistAsync(
            int userId);

        Task AddAsync(
            Wishlist wishlist);

        void Remove(
            Wishlist wishlist);

        Task SaveChangesAsync();
    }
}