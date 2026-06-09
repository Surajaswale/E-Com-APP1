using ECommerceSolution.DTOs.Payment;

namespace ECommerceSolution.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentResponseDto>
            ProcessPaymentAsync(
                int userId,
                ProcessPaymentDto dto);
    }
}