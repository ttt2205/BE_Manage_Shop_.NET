using Manage_Store.Exceptions;
using Manage_Store.Models.Dtos;
using Manage_Store.Models.Entities;
using Manage_Store.Models.Requests;
using Manage_Store.Responses;
using Manage_Store.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Manage_Store.Controllers
{
    [ApiController]
    [Route("api/v1/category")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: api/category
        [HttpGet]
        public async Task<IActionResult> GetPaginationCategories([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? name = null)
        {
            var response = await _categoryService.GetPaginationAsync(page, pageSize);
            return Ok(response);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetCategories()
        {
            var response = await _categoryService.GetCategoriesAsync();
            var res = ApiResultResponse<Category>.Builder()
                    .WithSuccess(true)
                    .WithStatus(200)
                    .WithMessage("Lay danh sach category thanh cong")
                    .WithResult(response)
                    .Build();
            return Ok(res);
        }

        // GET: api/category/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.GetCategoryAsync(id);

            return Ok(ApiResponse<Category>.Builder()
                .WithSuccess(true)
                .WithStatus(200)
                .WithMessage("Lấy category thành công")
                .WithData(category)
                .Build());
        }


        // POST: api/category
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryReq categoryReq)
        {
            if (categoryReq == null)
            {
                throw new BadRequestException("Dữ liệu không hợp lệ");
            }
            var created = await _categoryService.CreateAsync(categoryReq);

            return Ok(ApiResponse<Category>.Builder()
                .WithSuccess(true)
                .WithStatus(201)
                .WithMessage("Tạo category thành công")
                .WithData(created)
                .Build());
        }


        // PUT: api/category/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryReq categoryReq)
        {

            var category = await _categoryService.GetCategoryAsync(id);
            if (category == null)
                throw new NotFoundException("Category không tồn tại");

            var updated = await _categoryService.UpdateAsync(id, categoryReq);
            return Ok(ApiResponse<Category>.Builder()
                .WithSuccess(true)
                .WithStatus(200)
                .WithMessage("Cập nhật category thành công")
                .WithData(updated)
                .Build());
        }

        // DELETE: api/category/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryService.GetCategoryAsync(id);
            if (category == null)
                throw new BadRequestException("Category không tồn tại");

            await _categoryService.DeleteAsync(id);
            return Ok(ApiResponse<string>.Builder()
            .WithSuccess(true)
            .WithStatus(200)
            .WithMessage("Xóa category thành công")
            .Build());
        }
    }
}
