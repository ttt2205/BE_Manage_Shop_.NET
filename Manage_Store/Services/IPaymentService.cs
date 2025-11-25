using Manage_Store.Models.Entities;
using Manage_Store.Models.Requests;


namespace Manage_Store.Services
{
    public interface IPaymentService
    {
        Task<Payment> CreateAsync(PaymentReq paymentReq);
    }
}
