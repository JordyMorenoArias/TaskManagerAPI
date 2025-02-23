using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Data;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TaskManagerContext context;

        public UserRepository(TaskManagerContext context)
        {
            this.context = context;
        }

        public async Task<User?> GetUserById(int userId)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User?> Create(User user)
        {
            var result = await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<User?> Update(User user)
        {
            var result = context.Users.Update(user);
            await context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<User?> Delete(int userId)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return null;
            }
            context.Users.Remove(user);
            await context.SaveChangesAsync();
            return user;
        }

    }
}
