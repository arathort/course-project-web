using System.ComponentModel.DataAnnotations;

namespace Barabas.Models
{
    public class User(int Id, string Username, string Email, string Password_Hash, int RoleId)
    {
        public int Id { get; set; } = Id;
        public string Username { get; set; } = Username;
        [Required]
        public string Email { get; set; } = Email;
        [Required]
        public string Password_Hash { get; set; } = Password_Hash;
        [Required]
        public int RoleId { get; set; } = RoleId;
        public bool IsVerifiedOrganizer { get; set; } = false;

    }
}
