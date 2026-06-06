using TaskManager.API.Models;

namespace TaskManager.API.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        // Inherits: GetByIdAsync, GetAllAsync, Create, Update, Delete
        // Add user-specific methods here
        Task<User?> GetByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email);
    }
}