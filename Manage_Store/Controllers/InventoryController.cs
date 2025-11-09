using Manage_Store.Models.Dtos;
using Manage_Store.Models.Requests;
using Manage_Store.Services;
using Microsoft.AspNetCore.Mvc;

namespace Manage_Store.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _inventoryService.GetAllAsync(page, pageSize);
            return StatusCode(result.Status, result);
        }

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetByProductId(int productId)
        {
            var result = await _inventoryService.GetByProductIdAsync(productId);
            return StatusCode(result.Status, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateInventoryRequest request)
        {
            var result = await _inventoryService.UpdateInventoryAsync(request);
            return StatusCode(result.Status, result);
        }

        [HttpPost("import")]
        public async Task<IActionResult> Import([FromBody] ImportInventoryRequest request)
        {
            var result = await _inventoryService.ImportInventoryAsync(request);
            return StatusCode(result.Status, result);
        }

        [HttpGet("report")]
        public async Task<IActionResult> GetReport([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var result = await _inventoryService.GetInventoryReportAsync(startDate, endDate);
            return StatusCode(result.Status, result);
        }

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

                // Read Excel file using EPPlus or similar library
                // For now, return placeholder response
                // You'll need to install ClosedXML or EPPlus NuGet package

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
                return StatusCode(500, new { success = false, message = $"Error: {ex.Message}" });
            }
        }
    }
}