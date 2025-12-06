// Server/Controllers/TasksController.cs
using Application.Interfaces.Services;
using Core.Models.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace Server.Controllers
{
    [ApiController, Route("[controller]")]
    public sealed class TasksController(ITaskService taskService, IUserService userService) : BaseController
    {
        private readonly ITaskService _taskService = taskService;
        private readonly IUserService _userService = userService;

        [HttpPost]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> Create(TaskRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var createdTask = await _taskService.CreateAsync(request);
                return Ok(createdTask);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            string token = GetTokenFromHeaders();
            string id = await _userService.GetIdFromTokenAsync(token);
            return Ok(await _taskService.GetByUserId(id));
        }

        [HttpPut("{id}/complete")]
        [Authorize]
        public async Task<IActionResult> Complete(string id)
        {
            string currentUserId = GetCurrentUserId();
            try
            {
                await _taskService.Complete(id, currentUserId);
                return Ok();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<TaskResponse>> GetById(string id)
        {
            string currentUserId = GetCurrentUserId();
            var task = await _taskService.GetTaskByIdAsync(id, currentUserId);
            if (task == null) return NotFound();
            return Ok(task);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<TaskResponse>> Update(string id, [FromBody] TaskDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string currentUserId = GetCurrentUserId();
            var updatedTask = await _taskService.UpdateTaskAsync(id, request, currentUserId);
            if (updatedTask == null) return NotFound();
            return Ok(updatedTask);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            string currentUserId = GetCurrentUserId();
            var result = await _taskService.DeleteTaskAsync(id, currentUserId);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
