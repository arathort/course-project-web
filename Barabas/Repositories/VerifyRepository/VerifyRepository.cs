using Barabas.Data;
using Barabas.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Barabas.Repositories.VerifyRepository
{
    public class VerifyRepository(ApplicationDbContext context, UserManager<IdentityUser> userManager) : IVerifyRepository
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<IdentityUser> _userManager = userManager;

        public async Task<List<OrganizerVerificationHistory>> GetVerificationHistoryAsync(string userId)
        {
            return await _context.VerificationHistories
                .Where(x => x.UserId == userId)
                .Include(x => x.VerifiedByAdmin)
                .OrderByDescending(x => x.VerifiedAt)
                .ToListAsync();
        }

        public async Task<bool> VerifyOrganizerAsync(string userId, string adminId)
        {
            var identityUser = await _userManager.FindByIdAsync(userId);
            if (identityUser == null)
                return false;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == identityUser.Email);
            if (user == null)
                return false;

            user.IsVerifiedOrganizer = true;

            _context.VerificationHistories.Add(new OrganizerVerificationHistory
            {
                UserId = userId,
                VerifiedByAdminId = adminId,
                IsVerified = true,
                VerifiedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UnverifyOrganizerAsync(string userId, string adminId)
        {
            var identityUser = await _userManager.FindByIdAsync(userId);
            if (identityUser == null)
                return false;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == identityUser.Email);
            if (user == null)
                return false;

            user.IsVerifiedOrganizer = false;

            _context.VerificationHistories.Add(new OrganizerVerificationHistory
            {
                UserId = userId,
                VerifiedByAdminId = adminId,
                IsVerified = false,
                VerifiedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
