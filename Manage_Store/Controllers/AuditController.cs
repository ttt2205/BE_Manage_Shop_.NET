using Manage_Store.Models.Requests;
using Manage_Store.Responses;
using Manage_Store.Services;
using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Mvc;
using Manage_Store.Services.Documents;
using QuestPDF.Fluent;

namespace Manage_Store.Controllers
{
    [Route("api/audit")]
    [ApiController]
    public class AuditController : ControllerBase
    {
        private readonly IAuditService _auditService;
        private readonly ILogger<AuditController> _logger;

        public AuditController(IAuditService auditService, ILogger<AuditController> logger)
        {
            _auditService = auditService;
            _logger = logger;
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartSession([FromBody] CreateAuditSessionRequest request)
        {
            try
            {
                
                var mockUserId = 2; // Tạm thời dùng UserId = 2

                var session = await _auditService.StartAuditSessionAsync(request, mockUserId);
                
                var response = ApiResponse<object>.Builder()
                    .WithSuccess(true)
                    .WithStatus(201)
                    .WithMessage("Bắt đầu phiên kiểm kê thành công.")
                    .WithData(session)
                    .Build();
                return CreatedAtAction(nameof(GetSessionById), new { id = session.Id }, response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<object>.Builder().WithSuccess(false).WithStatus(400).WithMessage(ex.Message).Build());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi bắt đầu phiên kiểm kê.");
                return StatusCode(500, ApiResponse<object>.Builder().WithSuccess(false).WithStatus(500).WithMessage("Lỗi máy chủ nội bộ.").Build());
            }
        }
        
        
        [HttpPost("item")]
        public async Task<IActionResult> SubmitItem([FromBody] SubmitAuditItemRequest request)
        {
             if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<object>.Builder().WithSuccess(false).WithStatus(400).WithMessage("Dữ liệu không hợp lệ.").WithData(ModelState).Build());
            }

            try
            {
                var item = await _auditService.SubmitAuditItemAsync(request);
                var response = ApiResponse<object>.Builder()
                    .WithSuccess(true)
                    .WithStatus(200)
                    .WithMessage("Cập nhật mục kiểm kê thành công.")
                    .WithData(item)
                    .Build();
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<object>.Builder().WithSuccess(false).WithStatus(400).WithMessage(ex.Message).Build());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi gửi mục kiểm kê.");
                return StatusCode(500, ApiResponse<object>.Builder().WithSuccess(false).WithStatus(500).WithMessage("Lỗi máy chủ nội bộ.").Build());
            }
        }

        
        [HttpPost("finalize")]
        public async Task<IActionResult> FinalizeSession([FromBody] FinalizeAuditRequest request)
        {
            try
            {
                var session = await _auditService.FinalizeAuditSessionAsync(request);
                var response = ApiResponse<object>.Builder()
                    .WithSuccess(true)
                    .WithStatus(200)
                    .WithMessage("Đã chốt phiên kiểm kê và cập nhật kho thành công.")
                    .WithData(session)
                    .Build();
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<object>.Builder().WithSuccess(false).WithStatus(400).WithMessage(ex.Message).Build());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi chốt phiên kiểm kê.");
                return StatusCode(500, ApiResponse<object>.Builder().WithSuccess(false).WithStatus(500).WithMessage("Lỗi nghiêm trọng khi cập nhật kho.").Build());
            }
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSessionById(int id)
        {
            try
            {
                return StatusCode(501, ApiResponse<object>.Builder().WithSuccess(false).WithStatus(501).WithMessage("Endpoint chưa được triển khai.").Build());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy chi tiết phiên kiểm kê.");
                return StatusCode(500, ApiResponse<object>.Builder().WithSuccess(false).WithStatus(500).WithMessage("Lỗi máy chủ nội bộ.").Build());
            }
        }

        // [HttpGet("{id}/export-pdf")] // Route: GET /api/audit/1/export-pdf
        // public async Task<IActionResult> ExportAuditReport(int id)
        // {
        //     try
        //     {
        //         // 1. Lấy dữ liệu chi tiết
        //         var session = await _auditService.GetAuditSessionDetailsAsync(id);

        //         if (session == null)
        //         {
        //             var notFound = ApiResponse<object>.Builder()
        //                 .WithSuccess(false).WithStatus(404).WithMessage($"Không tìm thấy phiên kiểm kê ID: {id}").Build();
        //             return NotFound(notFound);
        //         }

        //         // 2. Tạo đối tượng Document
        //         var document = new AuditReportDocument(session);

        //         // 3. Tạo file PDF trong bộ nhớ
        //         byte[] pdfBytes = document.GeneratePdf();
                
        //         // 4. Đặt tên file và trả về
        //         string fileName = $"PhieuKiemKe_{session.Id}_{session.EndDate:yyyyMMdd}.pdf";
        //         return File(pdfBytes, "application/pdf", fileName);
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, "Lỗi khi xuất PDF cho phiên kiểm kê {AuditSessionId}", id);
        //         var error = ApiResponse<object>.Builder()
        //             .WithSuccess(false).WithStatus(500).WithMessage("Lỗi máy chủ khi tạo file PDF.").Build();
        //         return StatusCode(500, error);
        //     }
        // }
    }
}