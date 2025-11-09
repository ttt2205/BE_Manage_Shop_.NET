using Manage_Store.Models.Dtos;
using Manage_Store.Models.Requests;
using Manage_Store.Responses;

namespace Manage_Store.Services
{
    public interface ISupplierService
    {
        Task<ApiResPagination<List<SupplierDto>>> GetAllAsync(int page = 1, int pageSize = 10);
        Task<ApiResponse<SupplierDto>> GetByIdAsync(int id);
        Task<ApiResponse<SupplierDto>> CreateAsync(CreateSupplierRequest request);
        Task<ApiResponse<SupplierDto>> UpdateAsync(int id, UpdateSupplierRequest request);
        Task<ApiResponse<bool>> DeleteAsync(int id);
    }
}