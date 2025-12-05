using Application.Interfaces.Services;
using Core.Models.Users;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [ApiController, Route("[controller]")]
    public sealed class UsersController(IUserService userService) : BaseController
    {
        private readonly IUserService _usersService = userService;

        [HttpGet("me")]
        public async Task<ActionResult<UserResponse>> Get()
        {
            string token = GetTokenFromHeaders();

            var user = await _usersService.GetFromTokenAsync(token);
            return Ok(user);
        }

        [HttpGet]
        public Task<ActionResult<UserResponse>> Get(string id)
        {
            throw new NotImplementedException();
        }
    }
}
