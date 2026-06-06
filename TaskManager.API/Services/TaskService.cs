using TaskManager.API.DTOs.Tasks;
using TaskManager.API.Helpers;
using TaskManager.API.Models;
using TaskManager.API.Repositories.Interfaces;

namespace TaskManager.API.Services
{
    public class TaskService
    {
        private readonly ITaskRepository _taskRepo;

        public TaskService(ITaskRepository taskRepo)
        {
            _taskRepo = taskRepo;
        }

        public async Task<List<TaskResponseDto>> GetAllAsync(int userId)
        {
            var tasks = await _taskRepo.GetAllByUserIdAsync(userId);
            return tasks.Select(MapToDto).ToList();
        }

        public async Task<TaskResponseDto> GetByIdAsync(int id, int userId)
        {
            var task = await _taskRepo.GetByIdAndUserIdAsync(id, userId);
            if (task == null)
                throw new NotFoundException("Task", id);
            return MapToDto(task);
        }

        public async Task<TaskResponseDto> CreateAsync(
            CreateTaskDto dto, int userId)
        {
            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                Priority = dto.Priority,
                DueDate = dto.DueDate,
                UserId = userId
            };

            await _taskRepo.CreateAsync(task);
            return MapToDto(task);
        }

        public async Task<TaskResponseDto> UpdateAsync(
            int id, UpdateTaskDto dto, int userId)
        {
            var task = await _taskRepo.GetByIdAndUserIdAsync(id, userId);
            if (task == null)
                throw new NotFoundException("Task", id);

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.IsCompleted = dto.IsCompleted;
            task.Priority = dto.Priority;
            task.DueDate = dto.DueDate;

            await _taskRepo.UpdateAsync(task);
            return MapToDto(task);
        }

        public async Task DeleteAsync(int id, int userId)
        {
            var task = await _taskRepo.GetByIdAndUserIdAsync(id, userId);
            if (task == null)
                throw new NotFoundException("Task", id);

            await _taskRepo.DeleteAsync(task);
        }

        // Private mapper — keeps mapping logic in one place
        private static TaskResponseDto MapToDto(TaskItem task) =>
            new TaskResponseDto
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
}