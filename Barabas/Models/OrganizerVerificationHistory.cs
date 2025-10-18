using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Barabas.Models
{
    public class OrganizerVerificationHistory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; }

        [Required]
        public string VerifiedByAdminId { get; set; }

        [ForeignKey(nameof(VerifiedByAdminId))]
        public IdentityUser VerifiedByAdmin { get; set; }

        [Required]
        public bool IsVerified { get; set; }

        [Required]
        public DateTime VerifiedAt { get; set; } = DateTime.UtcNow;
    }
}
