using ECommerceSolution.DTOs.Wishlist;

namespace ECommerceSolution.Interfaces
{
    public interface IWishlistService
    {
        Task AddToWishlistAsync(
            int userId,
            AddWishlistDto dto);

        Task<List<WishlistResponseDto>>
            GetWishlistAsync(
                int userId);

        Task RemoveFromWishlistAsync(
            int userId,
            int productId);
    }
}