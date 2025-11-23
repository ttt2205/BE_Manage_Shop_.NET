using Manage_Store.Models.Entities;
using Manage_Store.Models.Requests;
using Manage_Store.Responses;
using Manage_Store.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Manage_Store.Controllers
{
    // localhost:****/api/auth
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PaymentReq paymentReq)
        {
            var created = await _paymentService.CreateAsync(paymentReq);

            return Ok(ApiResponse<Payment>.Builder()
                .WithSuccess(true)
                .WithStatus(201)
                .WithMessage("thanh toán thành công")
                .WithData(created)
                .Build());
        }


    }
}
