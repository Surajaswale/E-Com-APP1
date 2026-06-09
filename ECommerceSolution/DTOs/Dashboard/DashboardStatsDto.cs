namespace ECommerceSolution.DTOs.Dashboard
{
    public class DashboardStatsDto
    {
        public int TotalUsers { get; set; }

        public int TotalProducts { get; set; }

        public int TotalOrders { get; set; }

        public decimal TotalRevenue { get; set; }

        public int PendingOrders { get; set; }

        public int PaidOrders { get; set; }

        public int ShippedOrders { get; set; }

        public int DeliveredOrders { get; set; }

        public int CancelledOrders { get; set; }
    }
}