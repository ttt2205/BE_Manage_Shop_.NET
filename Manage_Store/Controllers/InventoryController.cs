// File: Controllers/InventoryController.cs
using Manage_Store.Models.Entities; 
using Manage_Store.Responses;       
using Manage_Store.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Manage_Store.Services.Documents;
using QuestPDF.Fluent;
﻿using Manage_Store.Models.Dtos;
using Manage_Store.Models.Requests;
using Manage_Store.Services;
using Microsoft.AspNetCore.Mvc;

namespace Manage_Store.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventory _inventory;
        private readonly ILogger<InventoryController> _logger;

        public InventoryController(IInventory inventory, ILogger<InventoryController> logger)
        {
            _inventory = inventory;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllInventory()
        {
            try
            {
                var inventoryItems = await _inventory.GetAllInventoryAsync();

                var response = ApiResponse<IEnumerable<Inventory>>.Builder()
                    .WithSuccess(true)
                    .WithStatus(StatusCodes.Status200OK) // 200
                    .WithMessage("Lấy danh sách tồn kho thành công.")
                    .WithData(inventoryItems)
                    .Build();

                return Ok(response);
            }
            catch (Exception ex)
            {
                // 1. Ghi lại lỗi
                _logger.LogError(ex, "Có lỗi xảy ra khi lấy danh sách tồn kho.");

                // 2. Xây dựng response thất bại bằng Builder
                // Chúng ta dùng <object> vì không có dữ liệu data cụ thể để trả về
                var errorResponse = ApiResponse<object>.Builder()
                    .WithSuccess(false)
                    .WithStatus(StatusCodes.Status500InternalServerError) // 500
                    .WithMessage("Có lỗi máy chủ nội bộ. Vui lòng thử lại sau.")
                    .WithData(null) // Không có dữ liệu khi lỗi
                    .Build();

                // 3. Trả về 500 với body là errorResponse
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }

        [HttpGet("export-pdf")] // Route: GET /api/Inventory/export-pdf
        public async Task<IActionResult> ExportInventoryToPdf()
        {
            try
            {
                // 1. Lấy dữ liệu (giống hệt như GetAll)
                var inventoryItems = await _inventory.GetAllInventoryAsync();
                
                // 2. Tạo instance của lớp Document và truyền dữ liệu vào
                var document = new InventoryPdfDocument(inventoryItems);

                // 3. Tạo file PDF trong bộ nhớ
                // (Bạn cũng có thể dùng GeneratePdfAsync nếu muốn)
                byte[] pdfBytes = document.GeneratePdf();

                // 4. Trả về file cho client
                // Client (trình duyệt) sẽ tự động mở hộp thoại "Save As..."
                string fileName = $"BaoCao_TonKho_{DateTime.Now:yyyyMMdd_HHmm}.pdf";
                
                return File(pdfBytes, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Có lỗi xảy ra khi xuất PDF tồn kho.");
                
                // Nếu lỗi, trả về JSON lỗi thay vì file
                var errorResponse = ApiResponse<object>.Builder()
                    .WithSuccess(false)
                    .WithStatus(StatusCodes.Status500InternalServerError)
                    .WithMessage("Không thể tạo file PDF do lỗi máy chủ.")
                    .Build();
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }
        // private readonly IInventoryService _inventoryService;

        // public InventoryController(IInventoryService inventoryService)
        // {
        //     _inventoryService = inventoryService;
        // }

        // [HttpGet]
        // public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        // {
        //     var result = await _inventoryService.GetAllAsync(page, pageSize);
        //     return StatusCode(result.Status, result);
        // }

        // [HttpGet("product/{productId}")]
        // public async Task<IActionResult> GetByProductId(int productId)
        // {
        //     var result = await _inventoryService.GetByProductIdAsync(productId);
        //     return StatusCode(result.Status, result);
        // }

        // [HttpPut]
        // public async Task<IActionResult> Update([FromBody] UpdateInventoryRequest request)
        // {
        //     var result = await _inventoryService.UpdateInventoryAsync(request);
        //     return StatusCode(result.Status, result);
        // }

        // [HttpPost("import")]
        // public async Task<IActionResult> Import([FromBody] ImportInventoryRequest request)
        // {
        //     var result = await _inventoryService.ImportInventoryAsync(request);
        //     return StatusCode(result.Status, result);
        // }

        // [HttpGet("report")]
        // public async Task<IActionResult> GetReport([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        // {
        //     var result = await _inventoryService.GetInventoryReportAsync(startDate, endDate);
        //     return StatusCode(result.Status, result);
        // }

        // [HttpPost("import-excel")]
        // public async Task<IActionResult> ImportFromExcel(IFormFile file, [FromForm] int userId)
        // {
        //     if (file == null || file.Length == 0)
        //     {
        //         return BadRequest(new { success = false, message = "No file uploaded" });
        //     }

        //     try
        //     {
        //         var items = new List<InventoryImportItemDto>();

        //         // Read Excel file using EPPlus or similar library
        //         // For now, return placeholder response
        //         // You'll need to install ClosedXML or EPPlus NuGet package

        //         var request = new ImportInventoryRequest
        //         {
        //             UserId = userId,
        //             Items = items
        //         };

        //         var result = await _inventoryService.ImportInventoryAsync(request);
        //         return StatusCode(result.Status, result);
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(500, new { success = false, message = $"Error: {ex.Message}" });
        //     }
        // }
    }
}