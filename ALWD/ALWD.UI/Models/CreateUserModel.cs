using System.ComponentModel.DataAnnotations;

namespace ALWD.UI.Models
{
    class CreateUserModel
    {
        public Dictionary<string, string> Attributes { get; set; } = new();
        public string Username { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public bool Enabled { get; set; } = true;
        public bool EmailVerified { get; set; } = true;
        public List<UserCredentials> Credentials { get; set; } = new();
    }
}
