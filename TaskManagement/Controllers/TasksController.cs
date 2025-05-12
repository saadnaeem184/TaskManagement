using Application.DTOs.TaskItemDTOs;
using Application.DTOs.TaskStatusUpdate;
using Application.Services.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TaskManagement.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<AppUser> _userManager;

        public TasksController(ITaskService taskService, IWebHostEnvironment env, UserManager<AppUser> userManager)
        {
            _taskService = taskService;
            _env = env;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var user = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(user);
            var isEmployer = roles.Contains("Employer");

            var tasks = isEmployer
                ? await _taskService.GetTasksForEmployerAsync()
                : await _taskService.GetTasksForEmployeeAsync(user.Id);

            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            return task == null ? NotFound() : Ok(task);
        }

        [HttpPost]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> CreateTask([FromForm] CreateTaskItemDto dto, IFormFile picture)
        {
            if (picture != null && picture.Length > 0)
            {
                var pictureUrl = await SaveImageAsync(picture); // OK here
                dto.PictureUrl = pictureUrl;
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _taskService.CreateTaskAsync(dto, userId);

            return Ok(new { message = "Task created successfully" });
        }

        [Authorize(Roles = "Employer")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateTaskItemDto dto, IFormFile? picture)
        {
            var user = await _userManager.GetUserAsync(User);

            // Save new image if provided
            if (picture != null && picture.Length > 0)
            {
                var pictureUrl = await SaveImageAsync(picture);
                dto.PictureUrl = pictureUrl;
            }

            var updated = await _taskService.UpdateTaskAsync(id, dto, user.Id);
            return updated ? NoContent() : Forbid();
        }

        [Authorize(Roles = "Employer")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var deleted = await _taskService.DeleteTaskAsync(id, user.Id);
            return deleted ? NoContent() : Forbid();
        }

        [Authorize(Roles = "Employee")]
        [HttpPost("{taskId}/status")]
        public async Task<IActionResult> UpdateStatus(int taskId, [FromForm] TaskStatusUpdateDto dto)
        {
            var user = await _userManager.GetUserAsync(User);
            var success = await _taskService.UpdateTaskStatusAsync(taskId, dto, user.Id);
            return success ? Ok() : Forbid();
        }

        private async Task<string> SaveImageAsync(IFormFile file)
        {
            var uploads = Path.Combine(_env.WebRootPath, "images");
            Directory.CreateDirectory(uploads);

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var path = Path.Combine(uploads, fileName);

            using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);

            return "/images/" + fileName;
        }
    }
}
