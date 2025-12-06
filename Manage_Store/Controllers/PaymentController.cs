using Manage_Store.Models.Entities;
using Manage_Store.Models.Requests;
using Manage_Store.Models.Dtos;
using Manage_Store.Responses;
using Manage_Store.Services;
using Microsoft.AspNetCore.Mvc;

namespace Manage_Store.Controllers
{
    // localhost:****/api/auth
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
    private readonly IMomoService _momoService;

        public PaymentController(IPaymentService paymentService, IMomoService momoService)
        {
            _paymentService = paymentService;
            _momoService = momoService;
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

        [HttpPost("momo")]
        public async Task<IActionResult> CreateMomoPayment([FromBody] OrderInfoDto model)
        {
            var payUrl = await _momoService.CreatePaymentUrl(model);
            return Ok(new { payUrl });
        }

        [HttpPost("momo-ipn")]
        public IActionResult MomoNotification([FromBody] MomoResultRequest request)
        {
            if (request.resultCode == 0)
            {
                Console.WriteLine("Thanh toán thành công đơn hàng: " + request.orderId);
            }
            else
            {   
                Console.WriteLine("Thanh toán thất bại: " + request.message);
            }

            return NoContent();
        }

    }
}
