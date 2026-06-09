using ECommerceSolution.DTOs.Cart;

namespace ECommerceSolution.Interfaces
{
    public interface ICartService
    {
        Task<CartResponseDto> GetCartAsync(int userId);

        Task AddToCartAsync(int userId, AddToCartDto dto);

        Task<bool> RemoveFromCartAsync(int userId, int productId);

        Task<bool> UpdateQuantityAsync(int userId, int productId, int quantity);
    }
}