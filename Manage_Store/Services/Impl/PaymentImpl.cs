using Manage_Store.Data;
using Manage_Store.Exceptions;
using Manage_Store.Models.Entities;
using Manage_Store.Models.Requests;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Manage_Store.Services.Impl
{
    public class PaymentImpl : IPaymentService
    {
        private readonly AppDbContext _context;

        public PaymentImpl(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Payment> CreateAsync(PaymentReq paymentReq)
        {
            // 🔍 Kiểm tra đơn hàng có tồn tại không
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == paymentReq.OrderId);

            if (order == null)
                throw new NotFoundException("Đơn hàng không tồn tại.");

            // 🔍 Kiểm tra trạng thái đơn hàng
            if (order.Status == "paid")
                throw new BadRequestException("Đơn hàng đã được thanh toán.");
            if (order.Status == "canceled")
                throw new BadRequestException("Đơn hàng đã huỷ.");

            var payment = new Payment
            {
                OrderId = paymentReq.OrderId,
                Amount = order.TotalAmount,
                PaymentMethod = paymentReq.PaymentMethod,
                PaymentDate = DateTime.Now
            };

            _context.Payments.Add(payment);

            // ✅ Cập nhật trạng thái đơn hàng
            order.Status = "paid";
            _context.Orders.Update(order);

            await _context.SaveChangesAsync();

            // Nạp lại Order navigation để trả ra response đầy đủ
            await _context.Entry(payment).Reference(p => p.Order).LoadAsync();

            return payment;
        }
    }
}
