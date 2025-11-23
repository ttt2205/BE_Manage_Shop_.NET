using Manage_Store.Data;
using Manage_Store.Models.Dtos;
using Manage_Store.Models.Entities;
using Manage_Store.Models.Requests;
using Microsoft.EntityFrameworkCore;

namespace Manage_Store.Services.Impl
{
    public class OrderImpl : IOrderService
    {
        private readonly AppDbContext _context;
        public OrderImpl(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Order> CreateAsync(OrderReq orderReq)
        {
            var order = new Order
            {
                CustomerId = orderReq.CustomerId,
                UserId = orderReq.UserId,
                PromotionId = orderReq.PromotionId,
                OrderDate = DateTime.Now,
                Status = orderReq.Status ?? "pending",
                Items = new List<OrderItem>()
            };

            decimal totalAmount = 0;

            // Tính tổng tiền từ các sản phẩm
            foreach (var itemDto in orderReq.Items)
            {
                var orderItem = new OrderItem
                {
                    ProductId = itemDto.ProductId,
                    Quantity = itemDto.Quantity,
                    Price = itemDto.Price,
                    Subtotal = itemDto.Price * itemDto.Quantity
                };

                totalAmount += orderItem.Subtotal;
                order.Items.Add(orderItem);
            }

            decimal discountAmount = 0;

            // Nếu có khuyến mãi
            if (orderReq.PromotionId.HasValue)
            {
                var promo = await _context.Promotions
                    .FirstOrDefaultAsync(p => p.Id == orderReq.PromotionId.Value);

                if (promo != null)
                {
                    // ✅ Kiểm tra điều kiện giá trị tối thiểu
                    if (totalAmount >= promo.MinOrderAmount)
                    {
                        if (promo.DiscountType == "percent")
                            discountAmount = totalAmount * (promo.DiscountValue / 100);
                        else if (promo.DiscountType == "amount")
                            discountAmount = promo.DiscountValue;

                        // Giới hạn discount không vượt tổng
                        if (discountAmount > totalAmount)
                            discountAmount = totalAmount;
                    }
                    else
                    {
                        // ❌ Không đủ điều kiện để áp dụng
                        discountAmount = 0;
                    }
                }
            }

            order.TotalAmount = totalAmount - discountAmount;
            order.DiscountAmount = discountAmount;

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Load các quan hệ
            // if (order.CustomerId.HasValue)
            //     await _context.Entry(order).Reference(p => p.Customer).LoadAsync();

            // if (order.UserId.HasValue)
            //     await _context.Entry(order).Reference(p => p.User).LoadAsync();

            // if (order.PromotionId.HasValue)
            //     await _context.Entry(order).Reference(p => p.Promotion).LoadAsync();

            return order;
        }


        public async Task<List<Order>> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.User)
                .Include(o => o.Promotion)
                .Include(o => o.Items) // nếu bạn muốn load các item trong đơn
                .ToListAsync();
        }



        public async Task<Order> GetOrderAsync(int id)
        {
            var order = await _context.Orders
                   .Include(o => o.Customer)
                   .Include(o => o.User)
                   .Include(o => o.Promotion)
                   .Include(o => o.Items)
                       .ThenInclude(i => i.Product)
                   .FirstOrDefaultAsync(o => o.Id == id);

            if (order.CustomerId.HasValue)
                await _context.Entry(order).Reference(p => p.Customer).LoadAsync();

            if (order.UserId.HasValue)
                await _context.Entry(order).Reference(p => p.User).LoadAsync();

            if (order.PromotionId.HasValue)
                await _context.Entry(order).Reference(p => p.Promotion).LoadAsync();


            return order;
        }

        public async Task<Order> UpdateAsync(int id, OrderReq orderReq)
        {
            var order = await _context.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == id);
            // Xóa các item cũ
            _context.OrderItems.RemoveRange(order.Items);
            order.Items.Clear();

            decimal totalAmount = 0;

            // Thêm lại item mới
            foreach (var itemDto in orderReq.Items)
            {
                var orderItem = new OrderItem
                {
                    OrderId = id,
                    ProductId = itemDto.ProductId,
                    Quantity = itemDto.Quantity,
                    Price = itemDto.Price,
                    Subtotal = itemDto.Price * itemDto.Quantity
                };

                totalAmount += orderItem.Subtotal;
                order.Items.Add(orderItem);
            }

            decimal discountAmount = 0;

            // Nếu có khuyến mãi
            if (orderReq.PromotionId.HasValue)
            {
                var promo = await _context.Promotions
                    .FirstOrDefaultAsync(p => p.Id == orderReq.PromotionId.Value);

                if (promo != null)
                {
                    if (totalAmount >= promo.MinOrderAmount)
                    {
                        if (promo.DiscountType == "percent")
                            discountAmount = totalAmount * (promo.DiscountValue / 100);
                        else if (promo.DiscountType == "amount")
                            discountAmount = promo.DiscountValue;
                        if (discountAmount > totalAmount)
                            discountAmount = totalAmount;
                    }
                    else
                    {
                        discountAmount = 0;
                    }
                }
            }
            order.PromotionId = orderReq.PromotionId;
            order.TotalAmount = totalAmount;
            order.DiscountAmount = discountAmount;

            await _context.SaveChangesAsync();

            return order;
        }

        public async Task<Order> UpdateStatus(int id, String status)
        {
            // Tìm đơn hàng theo ID
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
                throw new Exception("Order không tồn tại");

            // Cập nhật trạng thái mới
            order.Status = status;

            // Lưu thay đổi
            await _context.SaveChangesAsync();

            return order;
        }
    }
}
