using Barabas.Models;

namespace Barabas.Repositories.VerifyRepository
{
    public interface IVerifyRepository
    {
        Task<bool> VerifyOrganizerAsync(string userId, string adminId);
        Task<bool> UnverifyOrganizerAsync(string userId, string adminId);
        Task<List<OrganizerVerificationHistory>> GetVerificationHistoryAsync(string userId);
    }
}
