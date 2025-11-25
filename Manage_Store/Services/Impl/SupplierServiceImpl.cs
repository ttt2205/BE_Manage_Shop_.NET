using Manage_Store.Data;
using Manage_Store.Models.Dtos;
using Manage_Store.Models.Entities;
using Manage_Store.Models.Requests;
using Manage_Store.Responses;
using Microsoft.EntityFrameworkCore;

namespace Manage_Store.Services.Impl
{
    public class SupplierServiceImpl : ISupplierService
    {
        private readonly AppDbContext _context;

        public SupplierServiceImpl(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResPagination<List<SupplierDto>>> GetAllAsync(int page = 1, int pageSize = 10)
        {
            try
            {
                var totalItems = await _context.Suppliers.CountAsync();
                var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

                // FIX: Không dùng Builder trong Select - dùng object initializer trực tiếp
                var suppliers = await _context.Suppliers
                    .OrderBy(s => s.Id)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(s => new SupplierDto
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Phone = s.Phone,
                        Email = s.Email,
                        Address = s.Address
                    })
                    .ToListAsync();

                return ApiResPagination<List<SupplierDto>>.Builder()
                    .WithSuccess(true)
                    .WithStatus(200)
                    .WithMessage("Get suppliers successfully")
                    .WithResult(suppliers)
                    .WithMeta(new Meta
                    {
                        CurrentPage = page,
                        PageSize = pageSize,
                        TotalPage = totalPages,
                        TotalItems = totalItems
                    })
                    .Build();
            }
            catch (Exception ex)
            {
                return ApiResPagination<List<SupplierDto>>.Builder()
                    .WithSuccess(false)
                    .WithStatus(500)
                    .WithMessage($"Error: {ex.Message}")
                    .Build();
            }
        }

        public async Task<ApiResponse<SupplierDto>> GetByIdAsync(int id)
        {
            try
            {
                var supplier = await _context.Suppliers.FindAsync(id);

                if (supplier == null)
                {
                    return ApiResponse<SupplierDto>.Builder()
                        .WithSuccess(false)
                        .WithStatus(404)
                        .WithMessage("Supplier not found")
                        .Build();
                }

                // FIX: Tạo DTO sau khi query - không trong Select
                var dto = new SupplierDto
                {
                    Id = supplier.Id,
                    Name = supplier.Name,
                    Phone = supplier.Phone,
                    Email = supplier.Email,
                    Address = supplier.Address
                };

                return ApiResponse<SupplierDto>.Builder()
                    .WithSuccess(true)
                    .WithStatus(200)
                    .WithMessage("Get supplier successfully")
                    .WithData(dto)
                    .Build();
            }
            catch (Exception ex)
            {
                return ApiResponse<SupplierDto>.Builder()
                    .WithSuccess(false)
                    .WithStatus(500)
                    .WithMessage($"Error: {ex.Message}")
                    .Build();
            }
        }

        public async Task<ApiResponse<SupplierDto>> CreateAsync(CreateSupplierRequest request)
        {
            try
            {
                var supplier = new Supplier
                {
                    Name = request.Name,
                    Phone = request.Phone,
                    Email = request.Email,
                    Address = request.Address
                };

                _context.Suppliers.Add(supplier);
                await _context.SaveChangesAsync();

                var dto = new SupplierDto
                {
                    Id = supplier.Id,
                    Name = supplier.Name,
                    Phone = supplier.Phone,
                    Email = supplier.Email,
                    Address = supplier.Address
                };

                return ApiResponse<SupplierDto>.Builder()
                    .WithSuccess(true)
                    .WithStatus(201)
                    .WithMessage("Create supplier successfully")
                    .WithData(dto)
                    .Build();
            }
            catch (Exception ex)
            {
                return ApiResponse<SupplierDto>.Builder()
                    .WithSuccess(false)
                    .WithStatus(500)
                    .WithMessage($"Error: {ex.Message}")
                    .Build();
            }
        }

        public async Task<ApiResponse<SupplierDto>> UpdateAsync(int id, UpdateSupplierRequest request)
        {
            try
            {
                var supplier = await _context.Suppliers.FindAsync(id);

                if (supplier == null)
                {
                    return ApiResponse<SupplierDto>.Builder()
                        .WithSuccess(false)
                        .WithStatus(404)
                        .WithMessage("Supplier not found")
                        .Build();
                }

                supplier.Name = request.Name;
                supplier.Phone = request.Phone;
                supplier.Email = request.Email;
                supplier.Address = request.Address;

                await _context.SaveChangesAsync();

                var dto = new SupplierDto
                {
                    Id = supplier.Id,
                    Name = supplier.Name,
                    Phone = supplier.Phone,
                    Email = supplier.Email,
                    Address = supplier.Address
                };

                return ApiResponse<SupplierDto>.Builder()
                    .WithSuccess(true)
                    .WithStatus(200)
                    .WithMessage("Update supplier successfully")
                    .WithData(dto)
                    .Build();
            }
            catch (Exception ex)
            {
                return ApiResponse<SupplierDto>.Builder()
                    .WithSuccess(false)
                    .WithStatus(500)
                    .WithMessage($"Error: {ex.Message}")
                    .Build();
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            try
            {
                var supplier = await _context.Suppliers.FindAsync(id);

                if (supplier == null)
                {
                    return ApiResponse<bool>.Builder()
                        .WithSuccess(false)
                        .WithStatus(404)
                        .WithMessage("Supplier not found")
                        .Build();
                }

                _context.Suppliers.Remove(supplier);
                await _context.SaveChangesAsync();

                return ApiResponse<bool>.Builder()
                    .WithSuccess(true)
                    .WithStatus(200)
                    .WithMessage("Delete supplier successfully")
                    .WithData(true)
                    .Build();
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Builder()
                    .WithSuccess(false)
                    .WithStatus(500)
                    .WithMessage($"Error: {ex.Message}")
                    .Build();
            }
        }
    }
}