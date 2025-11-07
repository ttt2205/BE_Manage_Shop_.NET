using Manage_Store.Data;
using Manage_Store.Models.Entities;
using Manage_Store.Models.Requests;
using Microsoft.EntityFrameworkCore;

namespace Manage_Store.Services.Impl
{
    public class AuditServiceImpl : IAuditService
    {
        private readonly AppDbContext _context;

        public AuditServiceImpl(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AuditSessions> StartAuditSessionAsync(CreateAuditSessionRequest request, int userId)
        {
            var existingSession = await _context.AuditSessions
                .FirstOrDefaultAsync(s => s.UserId == userId && s.Status == "in_progress");

            if (existingSession != null)
            {
                throw new InvalidOperationException("Bạn đã có một phiên kiểm kê đang diễn ra. Vui lòng hoàn tất phiên đó trước.");
            }

            var session = new AuditSessions
            {
                UserId = userId,
                StartDate = DateTime.Now,
                Note = request.Note,
                Status = "in_progress" 
            };

            await _context.AuditSessions.AddAsync(session);
            await _context.SaveChangesAsync();
            return session;
        }

        public async Task<InventoryAuditItem> SubmitAuditItemAsync(SubmitAuditItemRequest request)
        {
            var session = await _context.AuditSessions.FindAsync(request.SessionId);
            if (session == null || session.Status != "in_progress")
            {
                throw new InvalidOperationException("Phiên kiểm kê không hợp lệ hoặc đã bị đóng.");
            }

            var inventory = await _context.Set<Inventory>()
                .FirstOrDefaultAsync(i => i.ProductId == request.ProductId);
            
            int systemQuantity = inventory?.Quantity ?? 0;

            var auditItem = await _context.InventoryAuditItems
                .FirstOrDefaultAsync(item => item.SessionId == request.SessionId && item.ProductId == request.ProductId);

            if (auditItem != null)
            {
                auditItem.ActualQuantity = request.ActualQuantity;
                auditItem.Note = request.Note;
                auditItem.Difference = request.ActualQuantity - auditItem.SystemQuantity;
            }
            else
            {
                
                auditItem = new InventoryAuditItem
                {
                    ProductId = request.ProductId,
                    SessionId = request.SessionId,
                    SystemQuantity = systemQuantity, 
                    ActualQuantity = request.ActualQuantity,
                    Difference = request.ActualQuantity - systemQuantity,
                    Note = request.Note
                };
                await _context.InventoryAuditItems.AddAsync(auditItem);
            }

            await _context.SaveChangesAsync();
            return auditItem;
        }

        public async Task<AuditSessions> FinalizeAuditSessionAsync(FinalizeAuditRequest request)
        {
            // --- BẮT ĐẦU TRANSACTION ---
            
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var session = await _context.AuditSessions
                    .Include(s => s.AuditItems) 
                    .FirstOrDefaultAsync(s => s.Id == request.SessionId);

                if (session == null || session.Status != "in_progress")
                {
                    throw new InvalidOperationException("Phiên không hợp lệ hoặc đã bị đóng.");
                }

               
                foreach (var item in session.AuditItems)
                {
                    var inventoryItem = await _context.Set<Inventory>()
                        .FirstOrDefaultAsync(i => i.ProductId == item.ProductId);

                    if (inventoryItem == null)
                    {
                       
                        inventoryItem = new Inventory
                        {
                            ProductId = item.ProductId,
                        };
                        await _context.Set<Inventory>().AddAsync(inventoryItem);
                    }
                    
                    
                    inventoryItem.Quantity = item.ActualQuantity; 
                    inventoryItem.UpdatedAt = DateTime.Now;
                }

                
                session.Status = "completed";
                session.EndDate = DateTime.Now;
                session.Note = string.IsNullOrEmpty(request.FinalNote) ? session.Note : request.FinalNote;

              
                await _context.SaveChangesAsync();
                
                
                await transaction.CommitAsync();

                return session;
            }
            catch (Exception)
            {
                
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<AuditSessions> CancelAuditSessionAsync(int sessionId)
        {
            var session = await _context.AuditSessions.FindAsync(sessionId);
            if (session == null || session.Status != "in_progress")
            {
                throw new InvalidOperationException("Không thể hủy phiên này.");
            }

            session.Status = "cancelled";
            session.EndDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return session;
        }
        
        public async Task<AuditSessions?> GetAuditSessionDetailsAsync(int sessionId)
        {
            return await _context.AuditSessions
                .Include(s => s.User) 
                .Include(s => s.AuditItems) 
                    .ThenInclude(item => item.Product) 
                .AsNoTracking() 
                .FirstOrDefaultAsync(s => s.Id == sessionId);
        }
    }
}