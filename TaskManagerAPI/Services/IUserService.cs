using TaskManagerAPI.Models;

namespace TaskManagerAPI.Services
{
    public interface IUserService
    {
        Task<User?> Update(User user);
    }
}