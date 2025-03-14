using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Notebook.Features;
using Notebook.Models;
using Notebook.Models.Requests;

namespace Notebook.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController(RegisterFeature registerFeature, LoginFeature loginFeature, LogoutFeature logoutFeature, GetAccountFeature getAccountFeature, UserManager<User> userManager, UpdateAccountFeature updateAccountFeature) : BaseController(userManager)
    {
        private readonly RegisterFeature _registerFeature = registerFeature;
        private readonly LoginFeature _loginFeature = loginFeature;
        private readonly LogoutFeature _logoutFeature = logoutFeature;
        private readonly GetAccountFeature _getAccountFeature = getAccountFeature;
        private readonly UpdateAccountFeature _updateAccountFeature = updateAccountFeature;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            try {
                var user = await _registerFeature.Execute(model);
                return UpdatedResponse(user.Id, "Registration successful");
            } catch (Exception e) {
                return BadRequest(new { Message = "An error occurred during registration", Error = e.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            var result = await _loginFeature.Execute(model);
            if (result.Succeeded)
            {
                return Ok(new { Message = "Login successful" });
            }
            else if (result.IsLockedOut)
            {
                return BadRequest(new { Message = "User account locked out" });
            }
            else if (result.IsNotAllowed)
            {
                return BadRequest(new { Message = "User is not allowed to sign in" });
            }
            else if (result.RequiresTwoFactor)
            {
                return BadRequest(new { Message = "Two-factor authentication is required" });
            }
            else
            {
                return BadRequest(new { Message = "Invalid login attempt" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get(User user)
        {
            var account = await _getAccountFeature.Execute(user);
            if (account == null)
            {
                return NotFound(new { Message = "Account not found" });
            }
            return ItemResponse(account);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(UpdateAccountRequest account, string id)
        {
            var user = await GetAuthenticatedUserAsync();
            var accountHasUpdated = await _updateAccountFeature.Execute(account, user);
            if (!accountHasUpdated)
            {
                return NotFound(new ErrorResponse { Message = "Account not found" });
            }
            return UpdatedResponse(id, "Account updated");
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _logoutFeature.Execute();
            return Ok(new { Message = "Logout successful" });
        }
    }
}