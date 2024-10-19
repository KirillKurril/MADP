using System.ComponentModel.DataAnnotations;

namespace ALWD.UI.Models
{
    class UserCredentials
    {
        public string Type { get; set; } = "password";
        public bool Temporary { get; set; } = false;

        [Required]
        public string Value { get; set; }
    }
}
