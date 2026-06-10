using ECommerceSolution.Data;
using ECommerceSolution.Entities;
using ECommerceSolution.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerceSolution.Repositories
{
    public class WishlistRepository
        : IWishlistRepository
    {
        private readonly AppDbContext _context;

        public WishlistRepository(
            AppDbContext context)
        {
            _context = context;
        }

        public async Task<Wishlist?>
            GetWishlistItemAsync(
                int userId,
                int productId)
        {
            return await _context.Wishlists
                .FirstOrDefaultAsync(w =>
                    w.UserId == userId &&
                    w.ProductId == productId);
        }

        public async Task<List<Wishlist>>
            GetUserWishlistAsync(
                int userId)
        {
            return await _context.Wishlists
                .Include(w => w.Product)
                .Where(w => w.UserId == userId)
                .ToListAsync();
        }

        public async Task AddAsync(
            Wishlist wishlist)
        {
            await _context.Wishlists
                .AddAsync(wishlist);
        }

        public void Remove(
            Wishlist wishlist)
        {
            _context.Wishlists.Remove(wishlist);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}