using Core.Models.Users;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [ApiController, Route("[controller]")]
    public sealed class UsersController : BaseController
    {
        [HttpGet("me")]
        public Task<ActionResult<UserResponse>> Get()
        {
            string token = GetTokenFromHeaders();

            throw new NotImplementedException();
            // в конце каждого типо
            // return Ok(user);
        }

        [HttpGet]
        public Task<ActionResult<UserResponse>> Get(string id)
        {
            throw new NotImplementedException();
        }
    }
}
