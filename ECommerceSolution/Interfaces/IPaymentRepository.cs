using ECommerceSolution.Entities;

namespace ECommerceSolution.Interfaces
{
    public interface IPaymentRepository
    {
        Task AddAsync(Payment payment);

        Task<Payment?> GetByOrderIdAsync(int orderId);

        Task SaveChangesAsync();
    }
}