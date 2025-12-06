using Application.Interfaces.Services;
using Application.Services;
using Core.Models.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [ApiController, Route("[controller]")]
    public sealed class TasksController(TaskService taskService, IUserService userService) : BaseController
    {
        private readonly TaskService _taskService = taskService;
        private readonly IUserService _userService = userService;

        [HttpPost]
        public async Task<IActionResult> Create(TaskRequest request)
        {
            string token = GetTokenFromHeaders();
            string id = await _userService.GetIdFromTokenAsync(token);
            await _taskService.CreateAsync(request, id);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _taskService.Get());
        }

        [HttpPut]
        public async Task<IActionResult> Complete(string id)
        {
            string token = GetTokenFromHeaders();
            string userId = await _userService.GetIdFromTokenAsync(token);

            await _taskService.Complete(id, userId);
            return Ok();
        }

        [HttpPut("take")]
        public async Task<IActionResult> Take(string id)
        {
            string token = GetTokenFromHeaders();
            string userId = await _userService.GetIdFromTokenAsync(token);
            
            await _taskService.TakeAsync(id, userId);
            return Ok();
        }
    }
}