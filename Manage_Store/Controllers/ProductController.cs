using Manage_Store.Models.Dtos;
using Manage_Store.Models.Entities;
using Manage_Store.Responses;
using Manage_Store.Services;
using Microsoft.AspNetCore.Mvc;
using Manage_Store.Exceptions;
using Manage_Store.Models.Requests;


namespace Manage_Store.Controllers
{

    [ApiController]
    [Route("api/v1/product")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _ProductService;

        public ProductController(IProductService ProductService)
        {
            _ProductService = ProductService;
        }

        // create
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductReq productReq)
        {
            if (productReq == null)
            {
                return BadRequest(ApiResponse<string>.Builder()
                    .WithSuccess(false)
                    .WithStatus(400)
                    .WithMessage("Dữ liệu không hợp lệ")
                    .Build());
            }
            var created = await _ProductService.CreateAsync(productReq);

            return Ok(ApiResponse<Product>.Builder()
                .WithSuccess(true)
                .WithStatus(201)
                .WithMessage("Tạo Product thành công")
                .WithData(created)
                .Build());
        }
        
        // get all
        [HttpGet]
        public async Task<IActionResult> GetAllProduct(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string search = null)
        {
            // Lấy toàn bộ danh sách từ service
            var allProducts = await _ProductService.GetPaginationAsync(search);
     
            var totalItems = allProducts.Count;
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            // Lấy dữ liệu cho trang hiện tại
            var pagedProducts = allProducts
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var meta = Meta.Builder()
                .WithCurrentPage(page)
                .WithPageSize(pageSize)
                .WithTotalItems(totalItems)
                .WithTotalPage(totalPages)
                .Build();

            return Ok(ApiResPagination<List<Product>>.Builder()
                .WithSuccess(true)
                .WithStatus(200)
                .WithMessage("Lấy danh sách sản phẩm thành công")
                .WithResult(pagedProducts)
                .WithMeta(meta)
                .Build());
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _ProductService.GetAllAsync();

            var res = ApiResultResponse<Product>.Builder()
                  .WithSuccess(true)
                  .WithStatus(200)
                  .WithMessage("Get products successfully")
                  .WithResult(data)
                  .Build();
            return Ok(res);
        }

        // get product by id

        [HttpGet("detail/{id}")]
        public async Task<IActionResult> GetProductByid(int id)
        {
            var product = await _ProductService.GetProductAsync(id);
            if (product == null)
                throw new BadRequestException("product không tồn tại");

            return Ok(ApiResponse<Product>.Builder()
                .WithSuccess(true)
                .WithStatus(200)
                .WithMessage("Lấy product thành công")
                .WithData(product)
                .Build());
        }

        // update
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductReq productReq)
        {
            var product = await _ProductService.GetProductAsync(id);
            if (product == null)
                throw new BadRequestException("product không tồn tại");

            var updated = await _ProductService.UpdateAsync(id, productReq);
            return Ok(ApiResponse<Product>.Builder()
                .WithSuccess(true)
                .WithStatus(200)
                .WithMessage("Cập nhật category thành công")
                .WithData(updated)
                .Build());
        }
        // delete
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _ProductService.GetProductAsync(id);
            if (product == null)
                throw new BadRequestException("product không tồn tại");

            await _ProductService.DeleteAsync(id);
            return Ok(ApiResponse<string>.Builder()
            .WithSuccess(true)
            .WithStatus(200)
            .WithMessage("Xóa product thành công")
            .Build());
        }
    }
}
