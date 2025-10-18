using Barabas.Models;
using Barabas.Repositories.VerifyRepository;

namespace Barabas.Services.VerificationService
{
    public class VerificationService( IVerifyRepository verifyRepository) : IVerificationService
    {
        private readonly IVerifyRepository _verifyRepository = verifyRepository;

        public Task<List<OrganizerVerificationHistory>> GetVerificationHistoryAsync(string userId)
        {
            return _verifyRepository.GetVerificationHistoryAsync(userId);
        }

        public async Task<bool> UnverifyOrganizerAsync(string userId, string adminId)
        {
            return await _verifyRepository.UnverifyOrganizerAsync(userId, adminId);
        }

        public async Task<bool> VerifyOrganizerAsync(string userId, string adminId)
        {
            return await _verifyRepository.VerifyOrganizerAsync(userId, adminId);
        }
    }
}
