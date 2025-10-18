using Barabas.Models;
using Barabas.Services.VerificationService;
using Barabas.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Barabas.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IVerificationService _verificationService;

        public UsersController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IVerificationService verificationService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _verificationService = verificationService;
        }

        // GET: Admin/Users
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        // GET: Admin/Users/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);
            var allRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();

            var viewModel = new EditUserViewModel
            {
                UserId = user.Id,
                Email = user.Email,
                Roles = roles.ToList(),
                AvailableRoles = allRoles
            };

            return View(viewModel);
        }

        // POST: Admin/Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null) return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);
            var selectedRoles = model.Roles ?? [];

            var rolesToRemove = userRoles.Except(selectedRoles);
            await _userManager.RemoveFromRolesAsync(user, rolesToRemove);

            var rolesToAdd = selectedRoles.Except(userRoles);
            await _userManager.AddToRolesAsync(user, rolesToAdd);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var history = await _verificationService.GetVerificationHistoryAsync(id);
            var roles = await _userManager.GetRolesAsync(user);

            foreach (var record in history)
            {
                var admin = await _userManager.FindByIdAsync(record.VerifiedByAdminId);
                record.VerifiedByAdmin ??= new IdentityUser { Email = admin?.Email ?? "Unknown" };
            }

            var lastAction = history.OrderByDescending(h => h.VerifiedAt).FirstOrDefault();

            ViewBag.History = history;
            ViewBag.IsVerified = lastAction?.IsVerified ?? false;
            ViewBag.IsOrganizer = roles.Contains("Manager");

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Verify(string userId)
        {
            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var success = await _verificationService.VerifyOrganizerAsync(userId, adminId);

            TempData["Message"] = success ? "Organizer verified successfully." : "Verification failed.";
            return RedirectToAction(nameof(Details), new { id = userId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unverify(string userId)
        {
            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var success = await _verificationService.UnverifyOrganizerAsync(userId, adminId);

            TempData["Message"] = success ? "Organizer unverified successfully." : "Unverification failed.";
            return RedirectToAction(nameof(Details), new { id = userId });
        }

    }
}
