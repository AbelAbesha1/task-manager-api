using Microsoft.EntityFrameworkCore;
using TaskManager.API.Data;
using TaskManager.API.DTOs.Tasks;
using TaskManager.API.Models;

namespace TaskManager.API.Services
{
    public class TaskService
    {
        private readonly AppDbContext _context;

        public TaskService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TaskResponseDto>> GetAllAsync(int userId)
        {
            return await _context.Tasks
                .Where(t => t.UserId == userId)
                .Select(t => new TaskResponseDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    IsCompleted = t.IsCompleted,
                    Priority = t.Priority,
                    DueDate = t.DueDate,
                    CreatedAt = t.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<TaskResponseDto?> GetByIdAsync(int id, int userId)
        {
            return await _context.Tasks
                .Where(t => t.Id == id && t.UserId == userId)
                .Select(t => new TaskResponseDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    IsCompleted = t.IsCompleted,
                    Priority = t.Priority,
                    DueDate = t.DueDate,
                    CreatedAt = t.CreatedAt
                })
                .FirstOrDefaultAsync();
        }

        public async Task<TaskResponseDto> CreateAsync(CreateTaskDto dto, int userId)
        {
            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                Priority = dto.Priority,
                DueDate = dto.DueDate,
                UserId = userId
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return new TaskResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
                Priority = task.Priority,
                DueDate = task.DueDate,
                CreatedAt = task.CreatedAt
            };
        }

        public async Task<TaskResponseDto?> UpdateAsync(int id, UpdateTaskDto dto, int userId)
        {
            var task = await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (task == null) return null;

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.IsCompleted = dto.IsCompleted;
            task.Priority = dto.Priority;
            task.DueDate = dto.DueDate;

            await _context.SaveChangesAsync();

            return new TaskResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
                Priority = task.Priority,
                DueDate = task.DueDate,
                CreatedAt = task.CreatedAt
            };
        }

        public async Task<bool> DeleteAsync(int id, int userId)
        {
            var task = await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (task == null) return false;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}