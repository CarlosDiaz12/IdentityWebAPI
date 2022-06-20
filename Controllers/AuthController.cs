using IdentityWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IdentityWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        public AuthController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel credential)
        {
            var result = await _signInManager.PasswordSignInAsync(credential.UserName, credential.Password, false, false);
            if (result.Succeeded) return Ok(new { success = true });

            return BadRequest(new { error = "Failed to login." });
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel dto)
        {
            var newUser = new IdentityUser { UserName = dto.UserName, NormalizedUserName = dto.UserName.ToUpper(), Email = dto.Email };
            var result = await _userManager.CreateAsync(newUser, dto.Password);

            if (result.Succeeded) return Ok(new { success = true });

            return BadRequest(new { error = "Failed to register new user." });
        }
    }
}
