using Application.Interfaces.Services;
using Core.Models.Users;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public sealed class AuthController(IUserService userService) : BaseController
    {
        private readonly IUserService _userService = userService;

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterRequest request)
        {
            await _userService.RegisterAsync(
                request.Name,
                request.Email,
                request.Password,
                request.Role
            );

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginRequest request)
        {
            string token = await _userService.LoginAsync(request.Email, request.Password);

            return Ok(token);
        }
    }
}
