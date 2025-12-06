using Manage_Store.Models.Dtos;
namespace Manage_Store.Services
{
    public interface IMomoService
    {
        Task<string> CreatePaymentUrl(OrderInfoDto model);
    }
}