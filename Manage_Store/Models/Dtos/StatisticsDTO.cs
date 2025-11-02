namespace Manage_Store.Models.Dtos
{
    public class RevenueStatistics
    {
        public string Period { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
        public int OrderCount { get; set; }
        public int TotalItemsSold { get; set; }
    }

    public class TopProduct
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int QuantitySold { get; set; }
        public decimal Revenue { get; set; }
    }

    public class DashboardSummary
    {
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public decimal AverageOrderValue { get; set; }
        public int TotalItemsSold { get; set; }
    }

    public class RevenueByDayResponse
    {
        public List<RevenueStatistics> Data { get; set; } = new List<RevenueStatistics>();
        public int TotalDays { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
    }

    public class RevenueByMonthResponse
    {
        public List<RevenueStatistics> Data { get; set; } = new List<RevenueStatistics>();
        public int TotalMonths { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public int? Year { get; set; }
        public int? SpecificMonth { get; set; }
    }

    public class RevenueByYearResponse
    {
        public List<RevenueStatistics> Data { get; set; } = new List<RevenueStatistics>();
        public int TotalYears { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
    }

    public class TopProductsResponse
    {
        public List<TopProduct> Data { get; set; } = new List<TopProduct>();
        public int TopCount { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}