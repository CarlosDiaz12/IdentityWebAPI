using System.ComponentModel.DataAnnotations;

namespace IdentityWebAPI.Models
{
    public class LoginModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Code { get; set; }
        public bool RemeberMe { get; set; }
        public bool RememberBrowser { get; set; }
    }
}
