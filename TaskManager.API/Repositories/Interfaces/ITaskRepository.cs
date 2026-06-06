using TaskManager.API.Models;

namespace TaskManager.API.Repositories.Interfaces
{
    public interface ITaskRepository : IRepository<TaskItem>
    {
        // Inherits: GetByIdAsync, GetAllAsync, Create, Update, Delete
        // Add task-specific methods here
        Task<List<TaskItem>> GetAllByUserIdAsync(int userId);
        Task<TaskItem?> GetByIdAndUserIdAsync(int id, int userId);
    }
}