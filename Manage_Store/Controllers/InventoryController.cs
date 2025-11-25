// Models & Dtos
using Manage_Store.Models.Dtos;
using Manage_Store.Models.Entities;
using Manage_Store.Models.Requests;
using Manage_Store.Responses;
// Services
using Manage_Store.Services;
using Manage_Store.Services.Documents; // Namespace chứa InventoryPdfDocument
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QuestPDF.Fluent;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manage_Store.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        // Sử dụng IInventoryService làm chuẩn chung
        private readonly IInventoryService _inventoryService;
        private readonly ILogger<InventoryController> _logger;

        public InventoryController(IInventoryService inventoryService, ILogger<InventoryController> logger)
        {
            _inventoryService = inventoryService;
            _logger = logger;
        }

        // 1. Lấy danh sách (Ưu tiên version có phân trang từ File 1)
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var result = await _inventoryService.GetAllAsync(page, pageSize);

                // Giả định result từ Service trả về object có thuộc tính Status (như File 1)
                // Nếu Service trả về data thô, hãy dùng ApiResponse.Builder như bên dưới
                return StatusCode(result.Status, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Có lỗi xảy ra khi lấy danh sách tồn kho.");
                return StatusCode(500, new { success = false, message = "Internal Server Error" });
            }
        }

        // 2. Lấy chi tiết theo ProductId
        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetByProductId(int productId)
        {
            var result = await _inventoryService.GetByProductIdAsync(productId);
            return StatusCode(result.Status, result);
        }

        // 3. Cập nhật tồn kho
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateInventoryRequest request)
        {
            var result = await _inventoryService.UpdateInventoryAsync(request);
            return StatusCode(result.Status, result);
        }

        // 4. Nhập kho (JSON)
        [HttpPost("import")]
        public async Task<IActionResult> Import([FromBody] ImportInventoryRequest request)
        {
            var result = await _inventoryService.ImportInventoryAsync(request);
            return StatusCode(result.Status, result);
        }

        // 5. Báo cáo tồn kho (Theo ngày)
        [HttpGet("report")]
        public async Task<IActionResult> GetReport([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var result = await _inventoryService.GetInventoryReportAsync(startDate, endDate);
            return StatusCode(result.Status, result);
        }

        // 6. Nhập kho từ Excel (File 1 + Log từ File 2)
        [HttpPost("import-excel")]
        public async Task<IActionResult> ImportFromExcel(IFormFile file, [FromForm] int userId)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { success = false, message = "No file uploaded" });
            }

            try
            {
                var items = new List<InventoryImportItemDto>();

                // TODO: Implement đọc file Excel (EPPlus/ClosedXML) tại đây
                // var loadedItems = ...

                var request = new ImportInventoryRequest
                {
                    UserId = userId,
                    Items = items
                };

                var result = await _inventoryService.ImportInventoryAsync(request);
                return StatusCode(result.Status, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi import Excel.");
                return StatusCode(500, new { success = false, message = $"Error: {ex.Message}" });
            }
        }

        // 7. Xuất PDF (Từ File 2)
        [HttpGet("export-pdf")]
        public async Task<IActionResult> ExportInventoryToPdf()
        {
            try
            {
                // 1. Lấy dữ liệu (Lấy số lượng lớn để in hết báo cáo)
                var result = await _inventoryService.GetAllAsync(1, 10000);

                // --- SỬA LỖI TẠI ĐÂY ---
                // Thay result.Data bằng result.Result (theo file ApiResPagination bạn gửi)
                var sourceData = result.Result;

                if (sourceData == null || !sourceData.Any())
                {
                    return BadRequest(new { success = false, message = "Không có dữ liệu để xuất PDF" });
                }

                // 2. Tạo PDF
                // Lưu ý: InventoryPdfDocument đã được sửa để nhận List<InventoryDto>
                var document = new InventoryPdfDocument(sourceData);

                byte[] pdfBytes = document.GeneratePdf();

                string fileName = $"BaoCao_TonKho_{DateTime.Now:yyyyMMdd_HHmm}.pdf";
                return File(pdfBytes, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Có lỗi xảy ra khi xuất PDF tồn kho.");

                // Trả về JSON lỗi
                var errorResponse = ApiResponse<object>.Builder()
                    .WithSuccess(false)
                    .WithStatus(StatusCodes.Status500InternalServerError)
                    .WithMessage("Không thể tạo file PDF do lỗi máy chủ.")
                    .Build();

                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }
    }
}