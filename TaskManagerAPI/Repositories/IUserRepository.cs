using TaskManagerAPI.Models;

namespace TaskManagerAPI.Repositories
{
    public interface IUserRepository
    {
        Task<User?> Delete(int userId);
        Task<User?> GetUserById(int userId);
        Task<User?> Update(User user);
    }
}