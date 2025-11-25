using Manage_Store.Models.Dtos;
using Manage_Store.Models.Entities;
using Manage_Store.Services;
using Microsoft.AspNetCore.Mvc;
using Manage_Store.Exceptions;
using Manage_Store.Responses;
using Manage_Store.Models.Requests;

namespace Manage_Store.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }


        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderReq orderReq)
        {
            if (orderReq == null || orderReq.Items == null || !orderReq.Items.Any())
                throw new BadRequestException("Đơn hàng không có sản phẩm nào.");

            var order = await _orderService.CreateAsync(orderReq);
            return Ok(ApiResponse<Order>.Builder()
               .WithSuccess(true)
               .WithStatus(201)
               .WithMessage("Tạo Đơn hàng thành công")
               .WithData(order)
               .Build());
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProduct()
        {
            var orders = await _orderService.GetAllAsync();
            if (orders == null || orders.Count == 0)
            {
                return NotFound(ApiResponse<string>.Builder()
                    .WithSuccess(false)
                    .WithStatus(404)
                    .WithMessage("Không có dữ liệu promotion")
                    .Build());
            }
            return Ok(ApiResponse<List<Order>>.Builder()
                .WithSuccess(true)
                .WithStatus(200)
                .WithMessage("Lấy danh sách orders thành công")
                .WithData(orders)
                .Build());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductByid(int id)
        {
            var order = await _orderService.GetOrderAsync(id);
            if (order == null)
                throw new BadRequestException("order không tồn tại");

            return Ok(ApiResponse<Order>.Builder()
                .WithSuccess(true)
                .WithStatus(200)
                .WithMessage("Lấy order thành công")
                .WithData(order)
                .Build());
        }

        // update
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdatePromotion(int id, [FromBody] OrderReq orderReq)
        {
            var order = await _orderService.GetOrderAsync(id);

            if (order == null)
                throw new BadRequestException("Đơn hàng không tồn tại");

            if (order.Status == "paid")
                throw new BadRequestException("Đơn hàng đã được thanh toán.");
            if (order.Status == "canceled")
                throw new BadRequestException("Đơn hàng đã huỷ.");

            var updated = await _orderService.UpdateAsync(id, orderReq);
            return Ok(ApiResponse<Order>.Builder()
                .WithSuccess(true)
                .WithStatus(200)
                .WithMessage("Cập nhật Đơn hàng thành công")
                .WithData(updated)
                .Build());
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] OrderReq orderReq)
        {
            var orderCuremt = await _orderService.GetOrderAsync(id);
            if (orderCuremt == null)
                throw new BadRequestException("Đơn hàng không tồn tại");
            if (orderCuremt.Status == "paid")
                throw new BadRequestException("Đơn hàng đã được thanh toán.");
            if (orderCuremt.Status == "canceled")
                throw new BadRequestException("Đơn hàng đã huỷ.");
            var order = await _orderService.UpdateStatus(id, orderReq.Status);
            return Ok(ApiResponse<Order>.Builder()
                .WithSuccess(true)
                .WithStatus(200)
                .WithMessage("Cập nhật trạng thái đơn hàng thành công")
                .WithData(order)
                .Build());
        }

        // GET: api/orders/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<Order>>> GetByUser(int userId)
        {
            var orders = await _orderService.GetOrdersByUserAsync(userId);
            return Ok(orders);
        }

    }
}
