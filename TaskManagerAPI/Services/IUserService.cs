using TaskManagerAPI.Models.User;

namespace TaskManagerAPI.Services
{
    public interface IUserService
    {
        Task<User?> Create(UserCreateDTO userCreateDTO);
        Task<User?> Delete(int userId);
        Task<User?> GetUserById(int userId);
        Task<User?> Update(UserUpdateDTO userUpdateDTO);
        Task<User?> Validate(string email, string password);
    }
}