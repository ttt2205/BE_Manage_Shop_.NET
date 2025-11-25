using Manage_Store.Models.Dtos;
using Manage_Store.Models.Requests;
using Manage_Store.Responses;
using Manage_Store.Models.Entities;

namespace Manage_Store.Services
{
    public interface IInventoryService
    {
        Task<ApiResPagination<List<InventoryDto>>> GetAllAsync(int page = 1, int pageSize = 10);
        Task<ApiResponse<InventoryDto>> GetByProductIdAsync(int productId);
        Task<ApiResponse<bool>> UpdateInventoryAsync(UpdateInventoryRequest request);
        Task<ApiResponse<bool>> ImportInventoryAsync(ImportInventoryRequest request);
        Task<ApiResponse<List<InventoryReportDto>>> GetInventoryReportAsync(DateTime? startDate, DateTime? endDate);
    }
}