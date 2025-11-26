using Manage_Store.Models.Dtos;
using Manage_Store.Responses;

namespace Manage_Store.Services
{
    public interface ICustomerService
    {
        Task<ApiResPagination<List<CustomerDto>>> GetPagination(int page, int size, string? search);
        Task<ApiResultResponse<CustomerDto>> GetAll();
        Task<ApiResponse<CustomerDto>> GetById(int id);
        Task<ApiResponse<CustomerDto>> CreateCustomer(CreateCustomerDto dto);
        Task<ApiResponse<CustomerDto>> UpdateCustomer(int id, UpdateCustomerDto dto);
        Task<ApiResponse<string>> DeleteCustomer(int id);
    }
}
