using Manage_Store.Models.Dtos;
using Manage_Store.Models.Entities;
using Manage_Store.Responses;
using Manage_Store.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Manage_Store.Exceptions;
using Manage_Store.Responses;
using Manage_Store.Models.Requests;

namespace Manage_Store.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class PromotionController : ControllerBase
    {
        private readonly IPromotionService _PromotionService;

        public PromotionController(IPromotionService PromotionService)
        {
            _PromotionService = PromotionService;
        }

        // create
        [HttpPost]
        public async Task<IActionResult> CreatePromotion([FromBody] PromotionReq promotionReq)
        {
            if (promotionReq == null)
            {
                return BadRequest(ApiResponse<string>.Builder()
                    .WithSuccess(false)
                    .WithStatus(400)
                    .WithMessage("Dữ liệu không hợp lệ")
                    .Build());
            }
            bool isExist = await _PromotionService.IsCodeExistsAsync(promotionReq.PromoCode);
            if (isExist)
            {
                throw new BadRequestException("Mã khuyến mãi đã tồn tại!");
            }

            var promotion = await _PromotionService.CreateAsync(promotionReq);

            return Ok(ApiResponse<Promotion>.Builder()
                .WithSuccess(true)
                .WithStatus(201)
                .WithMessage("Tạo promotion thành công")
                .WithData(promotion)
                .Build());
        }
        // get all
        [HttpGet]
        public async Task<IActionResult> GetAllProduct()
        {
            var promotions = await _PromotionService.GetAllAsync();
            if (promotions == null || promotions.Count == 0)
            {
                return NotFound(ApiResponse<string>.Builder()
                    .WithSuccess(false)
                    .WithStatus(404)
                    .WithMessage("Không có dữ liệu promotion")
                    .Build());
            }
            return Ok(ApiResponse<List<Promotion>>.Builder()
                .WithSuccess(true)
                .WithStatus(200)
                .WithMessage("Lấy danh sách promotions thành công")
                .WithData(promotions)
                .Build());
        }

        // get product by id

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPromotionByid(int id)
        {
            var promotion = await _PromotionService.GetPromotionAsync(id);
            if (promotion == null)
                throw new BadRequestException("promotion không tồn tại");

            return Ok(ApiResponse<Promotion>.Builder()
                .WithSuccess(true)
                .WithStatus(200)
                .WithMessage("Lấy promotion thành công")
                .WithData(promotion)
                .Build());
        }

        // update
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdatePromotion(int id, [FromBody] PromotionReq promotionReq)
        {
            var promotion = await _PromotionService.GetPromotionAsync(id);
            if (promotion == null)
                throw new BadRequestException("promotion không tồn tại");

            var updated = await _PromotionService.UpdateAsync(id, promotionReq);
            return Ok(ApiResponse<Promotion>.Builder()
                .WithSuccess(true)
                .WithStatus(200)
                .WithMessage("Cập nhật promotion thành công")
                .WithData(updated)
                .Build());
        }
        // delete
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var promotion = await _PromotionService.GetPromotionAsync(id);
            if (promotion == null)
                throw new BadRequestException("promotion không tồn tại");

            await _PromotionService.DeleteAsync(id);
            return Ok(ApiResponse<string>.Builder()
            .WithSuccess(true)
            .WithStatus(200)
            .WithMessage("Xóa promotion thành công")
            .Build());
        }

    }
}
