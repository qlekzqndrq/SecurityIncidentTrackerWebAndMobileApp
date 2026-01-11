using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using SecurityIncidentTrackerWebApp.Data;
using System.Threading.Tasks;
using SecurityIncidentTrackerWebApp.Models;

namespace SecurityIncidentTrackerWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager; // Avem nevoie de asta pentru Register

        // Constructor actualizat aducem si UserManager
        public AuthController(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        // LOGIN
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest("Lipsesc datele de login.");
            }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return Ok(new { message = "Login reusit!", email = model.Email });
            }

            return Unauthorized(new { message = "Email sau parola gresite." });
        }

        // REGISTER 
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
                return BadRequest("Date incomplete.");

            // Cream un user nou
            var user = new User { UserName = model.Email, Email = model.Email };

            // Il salvam in baza de date cu parola criptata
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return Ok(new { message = "Cont creat cu succes!" });
            }

            // Daca apar erori (ex: parola prea simpla, email existent)
            return BadRequest(result.Errors);
        }

        // LOGOUT 
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { message = "Delogat cu succes." });
        }
    }

    // Modelele pentru datele primite
    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RegisterModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        // Putem adauga si ConfirmPassword daca vrem 
    }
}