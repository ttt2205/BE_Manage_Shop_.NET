
using Manage_Store.Data;
using Manage_Store.Exceptions;
using Manage_Store.Models.Dtos;
using Manage_Store.Models.Entities;
using Manage_Store.Models.Requests;
using Manage_Store.Responses;
using Microsoft.EntityFrameworkCore;

namespace Manage_Store.Services.Impl
{
    public class UserServiceImpl : IUserService
    {
        private readonly AppDbContext _context;

        public UserServiceImpl(AppDbContext context)
        {
            _context = context;
        }

        // Lấy danh sách user phân trang + search
        public async Task<ApiResPagination<List<UserDto>>> GetUsers(int page, int size, string? search)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Username.Contains(search) || x.FullName!.Contains(search));
            }

            int totalItems = await query.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalItems / size);

            var users = await query
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * size)
                .Take(size)
                .Select(x => new UserDto
                {
                    Id = x.Id,
                    Username = x.Username,
                    FullName = x.FullName,
                    Role = x.Role
                }).ToListAsync();

            return ApiResPagination<List<UserDto>>.Builder()
                .WithSuccess(true)
                .WithStatus(200)
                .WithMessage("Lấy danh sách user thành công")
                .WithResult(users)
                .WithMeta(Meta.Builder()
                    .WithCurrentPage(page)
                    .WithPageSize(size)
                    .WithTotalItems(totalItems)
                    .WithTotalPage(totalPages)
                    .Build())
                .Build();
        }

        // Chi tiết user
        public async Task<ApiResponse<UserDto>> GetUserDetail(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return ApiResponse<UserDto>.Builder()
                    .WithSuccess(false)
                    .WithStatus(404)
                    .WithMessage("User không tồn tại")
                    .Build();

            return ApiResponse<UserDto>.Builder()
                .WithSuccess(true)
                .WithStatus(200)
                .WithData(UserDto.Builder()
                    .WithId(user.Id)
                    .WithUsername(user.Username)
                    .WithFullName(user.FullName ?? "")
                    .WithRole(user.Role)
                    .Build())
                .Build();
        }

        public async Task<ApiResPagination<List<OrderDto>>> GetUserOrders(int id)
        {
            var orders = await _context.Orders
    .Where(o => o.UserId == id)
    .Include(o => o.Items)                // Include OrderItem
    .ThenInclude(i => i.Product)          // Nếu cần lấy thêm Product
    .Select(o => new OrderDto
    {
        Id = o.Id,
        OrderDate = o.OrderDate,
        TotalAmount = o.TotalAmount,
        Status = o.Status,
        DiscountAmount = o.DiscountAmount,

        Items = o.Items.Select(i => new OrderItemDto
        {
            Id = i.Id,
            ProductId = i.ProductId,
            Quantity = i.Quantity,
            Price = i.Price,
            Subtotal = i.Subtotal,
            ProductName = i.Product != null ? i.Product.ProductName : null
        }).ToList()
    })
    .ToListAsync();

            return ApiResPagination<List<OrderDto>>.Builder()
                .WithSuccess(true)
                .WithStatus(200)
                .WithMessage("Lấy danh sách đơn hàng của user thành công")
                .WithResult(orders)
                .Build();
        }

        // Tạo user
        public async Task<ApiResponse<UserDto>> CreateUser(CreateUserDto dto)
        {
            if (await _context.Users.AnyAsync(x => x.Username == dto.Username))
            {
                throw new BadRequestException("Username đã tồn tại");
            }

            var user = new User
            {
                Username = dto.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                FullName = dto.FullName,
                Role = dto.Role,
                CreatedAt = DateTime.Now
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return ApiResponse<UserDto>.Builder()
                .WithSuccess(true)
                .WithStatus(201)
                .WithMessage("Tạo user thành công")
                .WithData(UserDto.Builder()
                    .WithId(user.Id)
                    .WithUsername(user.Username)
                    .WithFullName(user.FullName ?? "")
                    .WithRole(user.Role)
                    .Build())
                .Build();
        }

        // Cập nhật user
        public async Task<ApiResponse<UserDto>> UpdateUser(int id, UpdateUserDto dto)
        {
            if (dto.Username == null)
            {
                throw new BadRequestException("Username is requested!");
            }

            if (await _context.Users.AnyAsync(x => x.Username.ToLower() == dto.Username.Trim().ToLower() && x.Id != id))
            {
                throw new BadRequestException("Username đã tồn tại");
            }

            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return ApiResponse<UserDto>.Builder()
                    .WithSuccess(false)
                    .WithStatus(404)
                    .WithMessage("User không tồn tại")
                    .Build();

            if (user.Role == "admin")
            {
                return ApiResponse<UserDto>.Builder()
                    .WithSuccess(false)
                    .WithStatus(403)
                    .WithMessage("Không được thay đổi admin")
                    .Build();
            }

            user.Username = dto?.Username?.Trim() ?? user.Username;
            user.FullName = dto?.FullName?.Trim() ?? user.FullName;
            user.Role = dto.Role ?? user.Role;

            await _context.SaveChangesAsync();

            return ApiResponse<UserDto>.Builder()
                .WithSuccess(true)
                .WithStatus(200)
                .WithMessage("Cập nhật user thành công")
                .WithData(UserDto.Builder()
                    .WithId(user.Id)
                    .WithUsername(user.Username)
                    .WithFullName(user.FullName ?? "")
                    .WithRole(user.Role)
                    .Build())
                .Build();
        }

        // Xóa user (KHÔNG XÓA ADMIN)
        public async Task<ApiResponse<string>> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return ApiResponse<string>.Builder()
                    .WithSuccess(false)
                    .WithStatus(404)
                    .WithMessage("User không tồn tại")
                    .Build();

            if (user.Role == "admin")
                return ApiResponse<string>.Builder()
                    .WithSuccess(false)
                    .WithStatus(403)
                    .WithMessage("Không thể xóa admin")
                    .Build();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return ApiResponse<string>.Builder()
                .WithSuccess(true)
                .WithStatus(200)
                .WithMessage("Xóa user thành công")
                .WithData("OK")
                .Build();
        }
    }
}
