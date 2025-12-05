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
            await _taskService.CreateAsync(request);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            string token = GetTokenFromHeaders();
            string id = await _userService.GetIdFromTokenAsync(token);
            return Ok(await _taskService.GetByUserId(id));
        }

        [HttpPut]
        public async Task<IActionResult> Complete(string id)
        {
            await _taskService.Complete(id);
            return Ok();
        }
    }
}