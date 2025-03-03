using TaskManagerAPI.Models.User;

namespace TaskManagerAPI.Repositories
{
    public interface IUserRepository
    {
        Task<User?> Create(User user);
        Task<User?> Delete(int userId);
        Task<User?> GetUserByEmail(string email);
        Task<User?> GetUserById(int userId);
        Task<User?> Update(User user);
    }
}