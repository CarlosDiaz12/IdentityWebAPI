using IdentityWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IdentityWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TwoFactorAuthController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IPasswordHasher<IdentityUser> _passwordHasher;
        public TwoFactorAuthController(
            SignInManager<IdentityUser> signInManager, 
            UserManager<IdentityUser> userManager,
            IPasswordHasher<IdentityUser> passwordHasher
            )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _passwordHasher = passwordHasher;
        }
        [HttpGet("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid) return BadRequest("Validation error ocurred.");

            var user = await _userManager.FindByNameAsync(model.UserName);
            if(user != null)
            {
                if(_passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password) == PasswordVerificationResult.Success)
                {
                    var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, user.PhoneNumber);

                    // custom implementation
                    await SendSmsAsync(user.PhoneNumber, code);
                }
            }

            return BadRequest("Failed to login");
        }

        [HttpPost]
        public async Task<IActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest("Validation error ocurred.");

            var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(model.Code, model.RemeberMe, model.RememberBrowser);
            if (result.Succeeded) return Ok(new { message = "Code verified susccesfully." });

            return BadRequest(new { error = "Failed to Verify Code" });
        }


        private Task SendSmsAsync(string number, string message)
        {
            // implement SMS Service Like Twilio or Clickatell
            return Task.FromResult(0);
        }
    }
}
