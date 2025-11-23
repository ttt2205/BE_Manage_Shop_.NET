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
    [Route("api/[controller]")]
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
        public async Task<IActionResult> GetAllProduct()
        {
            var products = await _ProductService.GetAllAsync();
            if (products == null || products.Count == 0)
            {
                return NotFound(ApiResponse<string>.Builder()
                    .WithSuccess(false)
                    .WithStatus(404)
                    .WithMessage("Không có dữ liệu category")
                    .Build());
            }
            return Ok(ApiResponse<List<Product>>.Builder()
                .WithSuccess(true)
                .WithStatus(200)
                .WithMessage("Lấy danh sách category thành công")
                .WithData(products)
                .Build());
        }

        // get product by id

        [HttpGet("{id}")]
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
            .WithMessage("Xóa category thành công")
            .Build());
        }

    }
}
