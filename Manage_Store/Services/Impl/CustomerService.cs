using Manage_Store.Data;
using Manage_Store.Models.Dtos;
using Manage_Store.Models.Entities;
using Manage_Store.Responses;
using Microsoft.EntityFrameworkCore;

namespace Manage_Store.Services.Impl
{
    public class CustomerService : ICustomerService
    {
        private readonly AppDbContext _context;

        public CustomerService(AppDbContext context)
        {
            _context = context;
        }

        // Pagination
        public async Task<ApiResPagination<List<CustomerDto>>> GetPagination(int page, int size, string? search)
        {
            var query = _context.Customers.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Name.Contains(search) || x.Phone!.Contains(search));
            }

            int totalItems = await query.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalItems / size);

            var customers = await query
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * size)
                .Take(size)
                .Select(x => new CustomerDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Phone = x.Phone,
                    Email = x.Email,
                    Address = x.Address
                })
                .ToListAsync();

            return ApiResPagination<List<CustomerDto>>.Builder()
                .WithSuccess(true)
                .WithStatus(200)
                .WithMessage("Lấy danh sách khách hàng thành công")
                .WithResult(customers)
                .WithMeta(Meta.Builder()
                    .WithCurrentPage(page)
                    .WithPageSize(size)
                    .WithTotalItems(totalItems)
                    .WithTotalPage(totalPages)
                    .Build())
                .Build();
        }

        // Get all
        public async Task<ApiResultResponse<CustomerDto>> GetAll()
        {
            var customers = await _context.Customers
                .OrderByDescending(x => x.Id)
                .Select(x => new CustomerDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Phone = x.Phone,
                    Email = x.Email,
                    Address = x.Address
                })
                .ToListAsync();

            return ApiResultResponse<CustomerDto>.Builder()
                .WithSuccess(true)
                .WithStatus(200)
                .WithResult(customers)
                .Build();
        }

        // Get by ID
        public async Task<ApiResponse<CustomerDto>> GetById(int id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return ApiResponse<CustomerDto>.Builder()
                    .WithSuccess(false)
                    .WithStatus(404)
                    .WithMessage("Customer not found")
                    .Build();
            }

            return ApiResponse<CustomerDto>.Builder()
                .WithSuccess(true)
                .WithStatus(200)
                .WithData(new CustomerDto
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    Phone = customer.Phone,
                    Email = customer.Email,
                    Address = customer.Address
                })
                .Build();
        }

        // Create
        public async Task<ApiResponse<CustomerDto>> CreateCustomer(CreateCustomerDto dto)
        {
            var customer = new Customer
            {
                Name = dto.Name,
                Phone = dto.Phone,
                Email = dto.Email,
                Address = dto.Address,
                CreatedAt = DateTime.Now
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return ApiResponse<CustomerDto>.Builder()
                .WithSuccess(true)
                .WithStatus(201)
                .WithMessage("Create customer success")
                .WithData(new CustomerDto
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    Phone = customer.Phone,
                    Email = customer.Email,
                    Address = customer.Address
                })
                .Build();
        }

        // 📌 Update
        public async Task<ApiResponse<CustomerDto>> UpdateCustomer(int id, UpdateCustomerDto dto)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return ApiResponse<CustomerDto>.Builder()
                    .WithSuccess(false)
                    .WithStatus(404)
                    .WithMessage("Customer not found")
                    .Build();
            }

            customer.Name = dto.Name ?? customer.Name;
            customer.Phone = dto.Phone ?? customer.Phone;
            customer.Email = dto.Email ?? customer.Email;
            customer.Address = dto.Address ?? customer.Address;

            await _context.SaveChangesAsync();

            return ApiResponse<CustomerDto>.Builder()
                .WithSuccess(true)
                .WithStatus(200)
                .WithMessage("Update success")
                .WithData(new CustomerDto
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    Phone = customer.Phone,
                    Email = customer.Email,
                    Address = customer.Address
                })
                .Build();
        }

        // Delete
        public async Task<ApiResponse<string>> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return ApiResponse<string>.Builder()
                    .WithSuccess(false)
                    .WithStatus(404)
                    .WithMessage("Customer not found")
                    .Build();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return ApiResponse<string>.Builder()
                .WithSuccess(true)
                .WithStatus(200)
                .WithMessage("Delete success")
                .WithData("OK")
                .Build();
        }
    }
}
