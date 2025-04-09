using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Notebook.Constants;
using Notebook.Features;
using Notebook.Models;
using Notebook.Models.Domain;
using Notebook.Models.Requests;
using Notebook.Models.Responses;

namespace Notebook.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController(RegisterFeature registerFeature, LoginFeature loginFeature, LogoutFeature logoutFeature, UserManager<User> userManager, UpdateAccountFeature updateAccountFeature, GetAccountFeature getAccountFeature)
        : BaseController(userManager)
    {
        private readonly RegisterFeature _registerFeature = registerFeature;
        private readonly LoginFeature _loginFeature = loginFeature;
        private readonly LogoutFeature _logoutFeature = logoutFeature;
        private readonly UpdateAccountFeature _updateAccountFeature = updateAccountFeature;
        private readonly GetAccountFeature _getAccountFeature = getAccountFeature;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
                var (userId, result) = await _registerFeature.Execute(model);
                if (result.Succeeded) {
                    if (userId == null) return BadRequest(new { Message = ErrorMessages.ErrorOccurredDuringRegistration });
                    return Ok(userId);
                }
                return BadRequest(new { Message = ErrorMessages.ErrorOccurredDuringRegistration, Error = result.Errors.ToString() });
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
                return BadRequest(new { Message = ErrorMessages.UserLockedOut });
            }
            else if (result.IsNotAllowed)
            {
                return BadRequest(new { Message = ErrorMessages.UserNotAllowed });
            }
            else if (result.RequiresTwoFactor)
            {
                return BadRequest(new { Message = ErrorMessages.TwoFactorRequired });
            }
            else
            {
                return BadRequest(new { Message = ErrorMessages.InvalidLoginAttempt });
            }
        }

        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            return await HandleFeatureExecution((user) => _getAccountFeature.Execute(user));
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateAccountRequest account)
        {
            return await HandleFeatureExecution((user) => _updateAccountFeature.Execute(account, user));
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _logoutFeature.Execute();
            return Ok(new { Message = "Logout successful" });
        }
    }
}