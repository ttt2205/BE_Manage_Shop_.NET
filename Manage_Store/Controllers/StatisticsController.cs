using Microsoft.AspNetCore.Mvc;
using Manage_Store.Services;
using Manage_Store.Responses;
using Manage_Store.Models.Dtos;

namespace Manage_Store.Controllers
{
    [ApiController]
    [Route("api/v1/statistic")]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatistics _statistics;

        public StatisticsController(IStatistics statistics)
        {
            _statistics = statistics;
        }

        [HttpGet("dashboard-summary")]
        public async Task<ActionResult<ApiResponse<DashboardSummary>>> GetDashboardSummary()
        {
            try
            {
                var summary = await _statistics.GetDashboardSummaryAsync();
                
                return Ok(ApiResponse<DashboardSummary>.Builder()
                    .WithSuccess(true)
                    .WithStatus(200)
                    .WithMessage("Dashboard summary retrieved successfully")
                    .WithData(summary)
                    .Build());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<DashboardSummary>.Builder()
                    .WithSuccess(false)
                    .WithStatus(500)
                    .WithMessage($"Error retrieving dashboard summary: {ex.Message}")
                    .WithData(null)
                    .Build());
            }
        }

        [HttpGet("revenue-by-day")]
        public async Task<ActionResult<ApiResponse<RevenueByDayResponse>>> GetRevenueByDay(
            [FromQuery] DateTime? startDate = null, 
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var revenueData = await _statistics.GetRevenueByDayAsync(startDate, endDate);
                
                return Ok(ApiResponse<RevenueByDayResponse>.Builder()
                    .WithSuccess(true)
                    .WithStatus(200)
                    .WithMessage("Revenue by day retrieved successfully")
                    .WithData(revenueData)
                    .Build());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<RevenueByDayResponse>.Builder()
                    .WithSuccess(false)
                    .WithStatus(500)
                    .WithMessage($"Error retrieving revenue by day: {ex.Message}")
                    .WithData(null)
                    .Build());
            }
        }

        [HttpGet("revenue-by-month")]
        public async Task<ActionResult<ApiResponse<RevenueByMonthResponse>>> GetRevenueByMonth(
            [FromQuery] int? year = null,[FromQuery] int? specificMonth = null)
        {
            try
            {
                var revenueData = await _statistics.GetRevenueByMonthAsync(year, specificMonth);
                
                return Ok(ApiResponse<RevenueByMonthResponse>.Builder()
                    .WithSuccess(true)
                    .WithStatus(200)
                    .WithMessage("Revenue by month retrieved successfully")
                    .WithData(revenueData)
                    .Build());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<RevenueByMonthResponse>.Builder()
                    .WithSuccess(false)
                    .WithStatus(500)
                    .WithMessage($"Error retrieving revenue by month: {ex.Message}")
                    .WithData(null)
                    .Build());
            }
        }

        [HttpGet("revenue-by-year")]
        public async Task<ActionResult<ApiResponse<RevenueByYearResponse>>> GetRevenueByYear(
            [FromQuery] int? startYear = null, 
            [FromQuery] int? endYear = null)
        {
            try
            {
                var revenueData = await _statistics.GetRevenueByYearAsync(startYear, endYear);
                
                return Ok(ApiResponse<RevenueByYearResponse>.Builder()
                    .WithSuccess(true)
                    .WithStatus(200)
                    .WithMessage("Revenue by year retrieved successfully")
                    .WithData(revenueData)
                    .Build());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<RevenueByYearResponse>.Builder()
                    .WithSuccess(false)
                    .WithStatus(500)
                    .WithMessage($"Error retrieving revenue by year: {ex.Message}")
                    .WithData(null)
                    .Build());
            }
        }

        [HttpGet("top-products")]
        public async Task<ActionResult<ApiResponse<TopProductsResponse>>> GetTopProducts(
            [FromQuery] int topCount = 5, 
            [FromQuery] DateTime? startDate = null, 
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var topProducts = await _statistics.GetTopProductsAsync(topCount, startDate, endDate);
                
                return Ok(ApiResponse<TopProductsResponse>.Builder()
                    .WithSuccess(true)
                    .WithStatus(200)
                    .WithMessage("Top products retrieved successfully")
                    .WithData(topProducts)
                    .Build());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<TopProductsResponse>.Builder()
                    .WithSuccess(false)
                    .WithStatus(500)
                    .WithMessage($"Error retrieving top products: {ex.Message}")
                    .WithData(null)
                    .Build());
            }
        }
    }
}