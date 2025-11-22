using Manage_Store.Models.Entities;
using Manage_Store.Models.Requests;

namespace Manage_Store.Services
{
    public interface IAuditService
    {

        Task<IEnumerable<AuditSessions>> GetAllAuditSessionsAsync();
        Task<AuditSessions> StartAuditSessionAsync(CreateAuditSessionRequest request, int userId);
    
        Task<InventoryAuditItem> SubmitAuditItemAsync(SubmitAuditItemRequest request);
        
        Task<AuditSessions> FinalizeAuditSessionAsync(FinalizeAuditRequest request);

        Task<AuditSessions> CancelAuditSessionAsync(int sessionId);
        
        Task<AuditSessions?> GetAuditSessionDetailsAsync(int sessionId);
    }
}