using Manage_Store.Models.Dtos;

namespace Manage_Store.Services
{
    public interface IStatistics
    {
        Task<DashboardSummary> GetDashboardSummaryAsync();
        Task<RevenueByDayResponse> GetRevenueByDayAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<RevenueByMonthResponse> GetRevenueByMonthAsync(int? year = null,int? specificMonth = null);
        Task<RevenueByYearResponse> GetRevenueByYearAsync(int? startYear = null, int? endYear = null);
        Task<TopProductsResponse> GetTopProductsAsync(int topCount = 5, DateTime? startDate = null, DateTime? endDate = null);
    }
}