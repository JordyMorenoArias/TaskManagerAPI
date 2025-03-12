using TaskManagerAPI.Models.User;

namespace TaskManagerAPI.Services
{
    public interface IUserService
    {
        Task<User?> Create(UserCreateDTO userCreateDTO);
        Task<User?> Delete(int userId);
        int? GetAuthenticatedUserId(HttpContext httpContext);
        Task<User?> GetUserById(int userId);
        Task<User?> Update(UserUpdateDTO userUpdateDTO);
        Task<User?> Validate(string email, string password);
        Task<User?> Verify(string token);
    }
}