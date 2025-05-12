using Application.DTOs.TaskItemDTOs;
using Application.DTOs.TaskStatusUpdate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface ITaskService
    {
        Task<int> CreateTaskAsync(CreateTaskItemDto dto, string userId);
        Task<List<TaskItemDto>> GetTasksForEmployerAsync();
        Task<List<TaskItemDto>> GetTasksForEmployeeAsync(string employeeId);
        Task<TaskItemDto?> GetTaskByIdAsync(int id);
        Task<bool> UpdateTaskAsync(int id, UpdateTaskItemDto dto, string userId);
        Task<bool> DeleteTaskAsync(int id, string userId);
        Task<bool> UpdateTaskStatusAsync(int taskId, TaskStatusUpdateDto dto, string userId);
    }
}
