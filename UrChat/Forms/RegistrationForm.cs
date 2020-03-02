using System.ComponentModel.DataAnnotations;

namespace UrChat.Forms
{
    public class RegistrationForm
    {
        [Required]
        [MinLength(3)]
        public string Username { get; set; }
        [Required]
        [MinLength(3)]
        public string Password { get; set; }
    }
}