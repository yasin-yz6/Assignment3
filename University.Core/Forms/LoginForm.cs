using System.ComponentModel.DataAnnotations;

namespace University.Core.Forms
{
    public class LoginForm
    {
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
