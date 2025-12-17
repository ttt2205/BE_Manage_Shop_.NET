using Manage_Store.Data;
using Manage_Store.Exceptions;
using Manage_Store.Models.Dtos;
using Manage_Store.Models.Entities;
using Manage_Store.Models.Requests;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var order = new Order
                {
                    CustomerId = orderReq.CustomerId,
                    UserId = orderReq.UserId,
                    OrderDate = DateTime.Now,
                    Status = orderReq.Status ?? "pending",
                    Items = orderReq.Items.Select(i => new OrderItem
                    {
                        ProductId = i.ProductId,
                        Quantity = i.Quantity,
                        Price = i.Price,
                        Subtotal = i.Price * i.Quantity
                    }).ToList()
                };

                // ====== 1. Trừ tồn kho ======
                foreach (var item in order.Items)
                {
                    var inventory = await _context.Inventory
                        .FirstOrDefaultAsync(x => x.ProductId == item.ProductId);

                    if (inventory == null)
                        throw new BadRequestException($"Sản phẩm ID {item.ProductId} chưa có trong kho.");

                    if (inventory.Quantity < item.Quantity)
                        throw new BadRequestException(
                            $"Sản phẩm ID {item.ProductId} không đủ hàng. Còn {inventory.Quantity}."
                        );

                    inventory.Quantity -= item.Quantity;
                    inventory.UpdatedAt = DateTime.Now;
                }

                // ====== 2. Tính tiền ======
                decimal totalAmount = order.Items.Sum(i => i.Subtotal);
                decimal discountAmount = 0;

                if (orderReq.PromotionId.HasValue)
                {
                    var promo = await _context.Promotions
                        .FirstOrDefaultAsync(p => p.Id == orderReq.PromotionId.Value);

                    if (promo == null)
                        throw new BadRequestException("Khuyến mãi không tồn tại.");

                    if (promo.UsageLimit > 0 && promo.UsedCount >= promo.UsageLimit)
                        throw new BadRequestException($"Khuyến mãi '{promo.PromoCode}' đã hết lượt sử dụng.");

                    if (totalAmount >= promo.MinOrderAmount)
                    {
                        discountAmount = promo.DiscountType switch
                        {
                            "percent" => totalAmount * (promo.DiscountValue / 100),
                            "amount" => promo.DiscountValue,
                            _ => 0
                        };

                        if (discountAmount > totalAmount)
                            discountAmount = totalAmount;

                        promo.UsedCount++;
                        order.PromotionId = promo.Id;
                    }
                }

                order.TotalAmount = totalAmount - discountAmount;
                order.DiscountAmount = discountAmount;

                // ====== 3. Lưu Order ======
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // ====== 4. Commit ======
                await transaction.CommitAsync();

                return order;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


        public async Task<List<OrderDto>> GetAllAsync()
        {

            var orders = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.User)
                .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                .ToListAsync();

            return orders.Select(o => new OrderDto
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                Status = o.Status,
                TotalAmount = o.TotalAmount,
                DiscountAmount = o.DiscountAmount,
                Customer = o.Customer == null ? null : new CustomerDto
                {
                    Id = o.Customer.Id,
                    Name = o.Customer.Name,
                    Phone = o.Customer.Phone,
                    Email = o.Customer.Email,
                    Address = o.Customer.Address,
                },
                User = o.User == null ? null : new UserDto
                {
                    Username = o.User.Username,
                    FullName = o.User.FullName,
                    Role = o.User.Role,
                },
                Items = o.Items.Select(i => new OrderItemDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ProductName = i.Product?.ProductName ?? "",
                    Quantity = i.Quantity,
                    Price = i.Price,
                    Subtotal = i.Subtotal
                }).ToList()
            }).ToList();
        }

        public async Task<OrderDto?> GetOrderAsync(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.User)
                .Include(o => o.Promotion)
                .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return null; // hoặc throw new NotFoundException("Order not found");

            var orderDto = new OrderDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                DiscountAmount = order.DiscountAmount,
                Customer = order.Customer == null ? null : new CustomerDto
                {
                    Id = order.Customer.Id,
                    Name = order.Customer.Name,
                    Phone = order.Customer.Phone,
                    Email = order.Customer.Email,
                    Address = order.Customer.Address,
                },
                User = order.User == null ? null : new UserDto
                {
                    Username = order.User.Username,
                    FullName = order.User.FullName,
                    Role = order.User.Role,
                },
                Items = order.Items.Select(i => new OrderItemDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ProductName = i.Product?.ProductName ?? "",
                    Quantity = i.Quantity,
                    Price = i.Price,
                    Subtotal = i.Subtotal
                }).ToList()
            };

            return orderDto;
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

        public async Task<Order> UpdateStatus(int id, string status)
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

        public async Task<List<OrderDto>> GetOrdersByDateAsync(DateTime date)
        {
            var startDate = date.Date;
            var endDate = startDate.AddDays(1);

            var orders = await _context.Orders
                .Where(o => o.OrderDate >= startDate && o.OrderDate < endDate)
                .Include(o => o.Customer)
                .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                .ToListAsync();

            return orders.Select(o => new OrderDto
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                Status = o.Status,
                TotalAmount = o.TotalAmount,
                DiscountAmount = o.DiscountAmount,
                Customer = o.Customer == null ? null : new CustomerDto
                {
                    Id = o.Customer.Id,
                    Name = o.Customer.Name,
                    Phone = o.Customer.Phone,
                    Email = o.Customer.Email,
                    Address = o.Customer.Address,
                },
                Items = o.Items.Select(i => new OrderItemDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ProductName = i.Product?.ProductName ?? "",
                    Quantity = i.Quantity,
                    Price = i.Price,
                    Subtotal = i.Subtotal
                }).ToList()
            }).ToList();
        }



    }
}
