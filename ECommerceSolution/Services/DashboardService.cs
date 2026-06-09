using ECommerceSolution.Data;
using ECommerceSolution.DTOs.Dashboard;
using ECommerceSolution.Entities;
using ECommerceSolution.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerceSolution.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly AppDbContext _context;

        public DashboardService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardStatsDto> GetStatsAsync()
        {
            return new DashboardStatsDto
            {
                TotalUsers =
                    await _context.Users.CountAsync(),

                TotalProducts =
                    await _context.Products.CountAsync(),

                TotalOrders =
                    await _context.Orders.CountAsync(),

                TotalRevenue =
                    await _context.Orders
                        .Where(o =>
                            o.Status == OrderStatus.Paid ||
                            o.Status == OrderStatus.Shipped ||
                            o.Status == OrderStatus.Delivered)
                        .SumAsync(o => (decimal?)o.TotalAmount)
                        ?? 0,

                PendingOrders =
                    await _context.Orders
                        .CountAsync(o =>
                            o.Status == OrderStatus.Pending),

                PaidOrders =
                    await _context.Orders
                        .CountAsync(o =>
                            o.Status == OrderStatus.Paid),

                ShippedOrders =
                    await _context.Orders
                        .CountAsync(o =>
                            o.Status == OrderStatus.Shipped),

                DeliveredOrders =
                    await _context.Orders
                        .CountAsync(o =>
                            o.Status == OrderStatus.Delivered),

                CancelledOrders =
                    await _context.Orders
                        .CountAsync(o =>
                            o.Status == OrderStatus.Cancelled)
            };
        }
    }
}