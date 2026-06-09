using ECommerceSolution.DTOs.Order;

namespace ECommerceSolution.Interfaces
{
    public interface IOrderService
    {
        Task<OrderResponseDto> CheckoutAsync(int userId);

        Task<List<OrderResponseDto>> GetMyOrdersAsync(int userId);



        Task<OrderResponseDto?> GetOrderByIdAsync(
            int orderId,
            int userId);

        Task<List<OrderResponseDto>> GetAllOrdersAsync();

        Task UpdateOrderStatusAsync(
            UpdateOrderStatusDto dto);
    }
}