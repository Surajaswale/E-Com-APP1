using ECommerceSolution.Entities;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(int orderId);

    Task<List<Order>> GetAllAsync();

    Task<List<Order>> GetByUserIdAsync(int userId);

    Task AddAsync(Order order);

    void Update(Order order);

    Task SaveChangesAsync();
}