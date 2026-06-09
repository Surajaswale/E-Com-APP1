using ECommerceSolution.DTOs.Dashboard;

namespace ECommerceSolution.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardStatsDto> GetStatsAsync();
    }
}   