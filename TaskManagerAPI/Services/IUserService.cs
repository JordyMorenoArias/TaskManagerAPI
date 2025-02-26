using TaskManagerAPI.Models;

namespace TaskManagerAPI.Services
{
    public interface IUserService
    {
        Task<User?> Create(User user);
        Task<User?> Delete(int userId);
        Task<User?> Update(User user);
        Task<User?> Validate(string email, string password);
    }
}