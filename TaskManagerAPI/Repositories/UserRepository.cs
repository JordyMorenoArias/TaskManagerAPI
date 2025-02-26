using Microsoft.AspNetCore.Identity;
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

        /// <summary>
        /// Retrieves a user by their ID.
        /// </summary>
        /// <param name="userId">User ID.</param>
        /// <returns>The found user or null if not found.</returns>
        public async Task<User?> GetUserById(int userId)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        /// <summary>
        /// Retrieves a user by their email.
        /// </summary>
        /// <param name="email">User email.</param>
        /// <returns>The found user or null if not found.</returns>
        public async Task<User?> GetUserByEmail(string email)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        /// <summary>
        /// Creates a new user in the database.
        /// </summary>
        /// <param name="user">User object to create.</param>
        /// <returns>The created user.</returns>
        public async Task<User?> Create(User user)
        {
            var result = await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return result.Entity;
        }

        /// <summary>
        /// Updates an existing user's information.
        /// </summary>
        /// <param name="user">User object with updated information.</param>
        /// <returns>The updated user.</returns>
        public async Task<User?> Update(User user)
        {
            var result = context.Users.Update(user);
            await context.SaveChangesAsync();
            return result.Entity;
        }

        /// <summary>
        /// Deletes a user from the database.
        /// </summary>
        /// <param name="userId">ID of the user to delete.</param>
        /// <returns>The deleted user or null if not found.</returns>
        public async Task<User?> Delete(int userId)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return null;

            context.Users.Remove(user);
            await context.SaveChangesAsync();
            return user;
        }
    }
}
