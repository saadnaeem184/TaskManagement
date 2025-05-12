using Application.DTOs.TaskItemDTOs;
using Application.DTOs.TaskStatusUpdate;
using Application.Interfaces;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepo;
        private readonly UserManager<AppUser> _userManager;
        private readonly INotificationService _notificationService;

        public TaskService(ITaskRepository taskRepo, UserManager<AppUser> userManager,
            INotificationService notificationService)
        {
            _taskRepo = taskRepo;
            _userManager = userManager;
            _notificationService = notificationService;
        }

        public async Task<int> CreateTaskAsync(CreateTaskItemDto dto, string userId)
        {

            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                PictureUrl = dto.PictureUrl,
                AssigneeId = dto.AssigneeId,
                CreatedById = userId,
                RewardPrice = dto.RewardPrice
            };

            var createdTask =  await _taskRepo.CreateAsync(task);

            // Send notification to assigned employee
            SendNotfication(task.AssigneeId, task.Title);

            return createdTask;
        }

        public async Task<List<TaskItemDto>> GetTasksForEmployerAsync()
        {
            var tasks = await _taskRepo.GetAllForEmployerAsync();
            return tasks.Select(t => MapTask(t)).ToList();
        }

        public async Task<List<TaskItemDto>> GetTasksForEmployeeAsync(string employeeId)
        {
            var tasks = await _taskRepo.GetAllForEmployeeAsync(employeeId);
            return tasks.Select(t => MapTask(t)).ToList();
        }

        public async Task<TaskItemDto?> GetTaskByIdAsync(int id)
        {
            var task = await _taskRepo.GetByIdAsync(id);
            return task != null ? MapTask(task) : null;
        }

        public async Task<bool> UpdateTaskAsync(int id, UpdateTaskItemDto dto, string userId)
        {
            var task = await _taskRepo.GetByIdAsync(id);
            if (task == null || task.CreatedById != userId || task.AssigneeId != null) return false;

            if (!string.IsNullOrEmpty(dto.PictureUrl))
            {
                task.PictureUrl = dto.PictureUrl;
            }
            bool sendNotfication = false;
            if (string.IsNullOrEmpty(task.AssigneeId) && !string.IsNullOrEmpty(dto.AssigneeId))
            {
                sendNotfication = true;
            }

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.RewardPrice = dto.RewardPrice;
            task.AssigneeId = dto.AssigneeId;
            task.UpdatedAt = DateTime.UtcNow;
            task.Status = Enum.Parse<TaskItemStatus>(dto.Status.ToString());

            await _taskRepo.UpdateAsync(task);
            if (sendNotfication)
            {
                SendNotfication(task.AssigneeId, task.Title);
            }
            

            return true;
        }

        public async Task<bool> DeleteTaskAsync(int id, string userId)
        {
            var task = await _taskRepo.GetByIdAsync(id);
            if (task == null || task.CreatedById != userId || task.AssigneeId != null) return false;

            await _taskRepo.DeleteAsync(task);
            return true;
        }

        public async Task<bool> UpdateTaskStatusAsync(int taskId, TaskStatusUpdateDto dto, string userId)
        {
            var task = await _taskRepo.GetByIdAsync(taskId);
            if (task == null || task.AssigneeId != userId) return false;

            task.Status = Enum.Parse<TaskItemStatus>(dto.Status.ToString());
            task.StatusUpdates.Add(new TaskStatusUpdate
            {
                TaskItemId = task.Id,
                Comment = dto.Comment,
                Status = task.Status,
                UpdatedById = userId,
                UpdatedAt = DateTime.UtcNow
            });

            await _taskRepo.UpdateAsync(task);


            return true;
        }
        private async void SendNotfication(string AssigneeId, string Title)
        {
            // Send notification to assigned employee
            if (!string.IsNullOrWhiteSpace(AssigneeId))
            {
                var message = $"New task assigned: {Title}";
                await _notificationService.SendNotificationAsync(AssigneeId, message);
            }
        }
        private TaskItemDto MapTask(TaskItem task) => new TaskItemDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            PictureUrl = task.PictureUrl,
            AssigneeName = task.Assignee?.UserName,
            Status = task.Status.ToString(),
            RewardPrice = task.RewardPrice,
            CreatedByName = task.CreatedBy.UserName,
            CreatedAt = task.CreatedAt
        };
    }
}
