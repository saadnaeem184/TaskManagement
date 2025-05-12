using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAsync(TaskItem task)
        {
            _context.TaskItems.Add(task);
            await _context.SaveChangesAsync();
            return task.Id;
        }

        public async Task<TaskItem?> GetByIdAsync(int id) =>
            await _context.TaskItems.Include(t => t.CreatedBy).Include(t => t.Assignee).FirstOrDefaultAsync(t => t.Id == id);

        public async Task<List<TaskItem>> GetAllForEmployerAsync() =>
            await _context.TaskItems.Include(t => t.CreatedBy).Include(t => t.Assignee).ToListAsync();

        public async Task<List<TaskItem>> GetAllForEmployeeAsync(string employeeId) =>
            await _context.TaskItems
                .Where(t => t.AssigneeId == employeeId)
                .Include(t => t.CreatedBy)
                .ToListAsync();

        public async Task UpdateAsync(TaskItem task)
        {
            _context.TaskItems.Update(task);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TaskItem task)
        {
            _context.TaskItems.Remove(task);
            await _context.SaveChangesAsync();
        }
    }
}
