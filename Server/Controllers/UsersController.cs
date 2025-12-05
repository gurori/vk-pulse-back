using Core.Models.Users;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces.Services; 
using AutoMapper; 
using System;
using System.Collections.Generic; 
using System.Threading.Tasks;

namespace Server.Controllers
{
    [ApiController, Route("[controller]")]
    public sealed class UsersController : BaseController
    {
        private readonly IUserService _userService; 
        private readonly IMapper _mapper; 

        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("me")]
        public async Task<ActionResult<UserResponse>> GetCurrentUser()
        {
            Guid currentUserId;

            currentUserId = GetCurrentUserId();

            var userEntity = await _userService.GetUserByIdAsync(currentUserId);

            if (userEntity == null)
            {
                return NotFound("Current user not found.");
            }

            var userResponse = _mapper.Map<UserResponse>(userEntity);
            return Ok(userResponse);
        }

        [HttpGet("{id}")] 
        public async Task<ActionResult<UserResponse>> GetUserById(Guid id) 
        {
            var userEntity = await _userService.GetUserByIdAsync(id);

            if (userEntity == null)
            {
                return NotFound($"User with ID {id} not found.");
            }

            var userResponse = _mapper.Map<UserResponse>(userEntity);
            return Ok(userResponse);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetAllUsers()
        {
            var userEntities = await _userService.GetAllUsersAsync();
            var userResponses = _mapper.Map<IEnumerable<UserResponse>>(userEntities);
            return Ok(userResponses);
        }

        private Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return userId;
            }
            throw new UnauthorizedAccessException("User ID claim not found or invalid.");
        }
    }
}
