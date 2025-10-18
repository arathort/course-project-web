using Barabas.Models;

namespace Barabas.Services.VerificationService
{
    public interface IVerificationService
    {
        Task<bool> VerifyOrganizerAsync(string userId, string adminId);
        Task<bool> UnverifyOrganizerAsync(string userId, string adminId);
        Task<List<OrganizerVerificationHistory>> GetVerificationHistoryAsync(string userId);
    }
}
