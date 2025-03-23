using System.ComponentModel.DataAnnotations;

namespace WebStore.Models.Auth
{
    public class RegisterViewModel
    {
        [Required]
        [MaxLength(64)]
        public string Username { get; set; }

        [Required]
        [MaxLength(128)]
        public string Password { get; set; }

        [Required]
        [MaxLength(128)]
        public string PasswordAgain { get; set; }

        public RegisterViewModel()
        {
            Username = string.Empty;
            Password = string.Empty;
            PasswordAgain = string.Empty;
        }
    }
}
