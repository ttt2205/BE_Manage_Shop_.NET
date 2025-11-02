using Microsoft.EntityFrameworkCore;
using Manage_Store.Data;
using Manage_Store.Models.Dtos;

namespace Manage_Store.Services
{
    public class StatisticsImpl : IStatistics
    {
        private readonly AppDbContext _context;

        public StatisticsImpl(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardSummary> GetDashboardSummaryAsync()
        {
            var totalRevenue = await _context.Orders
                .Where(o => o.Status == "paid")
                .SumAsync(o => o.TotalAmount - o.DiscountAmount);

            var totalOrders = await _context.Orders
                .Where(o => o.Status == "paid")
                .CountAsync();

            var totalItemsSold = await _context.OrderItems
                .Join(_context.Orders,
                    oi => oi.OrderId,
                    o => o.Id,
                    (oi, o) => new { OrderItem = oi, Order = o })
                .Where(x => x.Order.Status == "paid")
                .SumAsync(x => x.OrderItem.Quantity);

            var averageOrderValue = totalOrders > 0 ? totalRevenue / totalOrders : 0;

            return new DashboardSummary
            {
                TotalRevenue = totalRevenue,
                TotalOrders = totalOrders,
                AverageOrderValue = averageOrderValue,
                TotalItemsSold = totalItemsSold
            };
        }

        public async Task<RevenueByDayResponse> GetRevenueByDayAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.Orders
                .Where(o => o.Status == "paid")
                .AsQueryable();

            if (startDate.HasValue)
                query = query.Where(o => o.OrderDate >= startDate.Value.Date);
            if (endDate.HasValue)
                query = query.Where(o => o.OrderDate <= endDate.Value.Date.AddDays(1).AddTicks(-1)); // Include entire end date

            var data = await query
                .GroupBy(o => o.OrderDate.Date)
                .Select(g => new 
                {
                    Date = g.Key,
                    Revenue = g.Sum(o => o.TotalAmount - o.DiscountAmount),
                    OrderCount = g.Count(),
                    TotalItemsSold = _context.OrderItems
                        .Where(oi => g.Select(o => o.Id).Contains(oi.OrderId))
                        .Sum(oi => oi.Quantity)
                })
                .OrderBy(x => x.Date)
                .ToListAsync();

            // Format period ở client side
            var result = data.Select(x => new RevenueStatistics
            {
                Period = x.Date.ToString("dd/MM/yyyy"),
                Revenue = x.Revenue,
                OrderCount = x.OrderCount,
                TotalItemsSold = x.TotalItemsSold
            }).ToList();

            return new RevenueByDayResponse
            {
                Data = result,
                TotalDays = result.Count,
                TotalRevenue = result.Sum(d => d.Revenue),
                TotalOrders = result.Sum(d => d.OrderCount)
            };
        }

        public async Task<RevenueByMonthResponse> GetRevenueByMonthAsync(int? year = null, int? specificMonth = null)
        {
            var query = _context.Orders
                .Where(o => o.Status == "paid")
                .AsQueryable();

            if (year.HasValue)
            {
                query = query.Where(o => o.OrderDate.Year == year.Value);
                
                // tìm theo tháng cụ thể
                if (specificMonth.HasValue && specificMonth.Value >= 1 && specificMonth.Value <= 12)
                {
                    query = query.Where(o => o.OrderDate.Month == specificMonth.Value);
                }
            }

            var data = await query
                .GroupBy(o => new { o.OrderDate.Year, o.OrderDate.Month })
                .Select(g => new 
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Revenue = g.Sum(o => o.TotalAmount - o.DiscountAmount),
                    OrderCount = g.Count(),
                    TotalItemsSold = _context.OrderItems
                        .Where(oi => g.Select(o => o.Id).Contains(oi.OrderId))
                        .Sum(oi => oi.Quantity)
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToListAsync();

            // Format period ở client side
            var result = data.Select(x => new RevenueStatistics
            {
                Period = $"{x.Month:00}/{x.Year}",
                Revenue = x.Revenue,
                OrderCount = x.OrderCount,
                TotalItemsSold = x.TotalItemsSold
            }).ToList();

            return new RevenueByMonthResponse
            {
                Data = result,
                TotalMonths = result.Count,
                TotalRevenue = result.Sum(d => d.Revenue),
                TotalOrders = result.Sum(d => d.OrderCount),
                Year = year,
                SpecificMonth = specificMonth  
            };
        }

        public async Task<RevenueByYearResponse> GetRevenueByYearAsync(int? startYear = null, int? endYear = null)
        {
            var query = _context.Orders
                .Where(o => o.Status == "paid")
                .AsQueryable();

            if (startYear.HasValue)
                query = query.Where(o => o.OrderDate.Year >= startYear.Value);
            if (endYear.HasValue)
                query = query.Where(o => o.OrderDate.Year <= endYear.Value);
            var data = await query
                .GroupBy(o => o.OrderDate.Year)
                .Select(g => new 
                {
                    Year = g.Key,
                    Revenue = g.Sum(o => o.TotalAmount - o.DiscountAmount),
                    OrderCount = g.Count(),
                    TotalItemsSold = _context.OrderItems
                        .Where(oi => g.Select(o => o.Id).Contains(oi.OrderId))
                        .Sum(oi => oi.Quantity)
                })
                .OrderBy(x => x.Year)
                .ToListAsync();

            // Format period ở client side
            var result = data.Select(x => new RevenueStatistics
            {
                Period = x.Year.ToString(),
                Revenue = x.Revenue,
                OrderCount = x.OrderCount,
                TotalItemsSold = x.TotalItemsSold
            }).ToList();

            return new RevenueByYearResponse
            {
                Data = result,
                TotalYears = result.Count,
                TotalRevenue = result.Sum(d => d.Revenue),
                TotalOrders = result.Sum(d => d.OrderCount)
            };
        }

        public async Task<TopProductsResponse> GetTopProductsAsync(int topCount = 5, DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.OrderItems
                .Include(oi => oi.Order)
                .Include(oi => oi.Product)
                .Where(oi => oi.Order!.Status == "paid") // ! để tránh null reference
                .AsQueryable();

            if (startDate.HasValue)
                query = query.Where(oi => oi.Order!.OrderDate >= startDate.Value);
            if (endDate.HasValue)
                query = query.Where(oi => oi.Order!.OrderDate <= endDate.Value);

            var data = await query
                .GroupBy(oi => new { oi.ProductId, oi.Product!.ProductName })
                .Select(g => new TopProduct
                {
                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.ProductName,
                    QuantitySold = g.Sum(oi => oi.Quantity),
                    Revenue = g.Sum(oi => oi.Subtotal)
                })
                .OrderByDescending(p => p.Revenue)
                .Take(topCount)
                .ToListAsync();

            return new TopProductsResponse
            {
                Data = data,
                TopCount = topCount,
                StartDate = startDate,
                EndDate = endDate,
                TotalRevenue = data.Sum(p => p.Revenue)
            };
        }
    }
}