namespace Barabas.ViewModels
{
    public class EditUserViewModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
        public List<string> AvailableRoles { get; set; } = new List<string>();
    }
}
