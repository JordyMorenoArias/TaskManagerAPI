using Microsoft.AspNetCore.Identity;
using TaskManagerAPI.Models;
using TaskManagerAPI.Repositories;

namespace TaskManagerAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        /// <summary>
        /// Validates a user's credentials.
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>The authenticated user if credentials are valid; otherwise, null.</returns>
        public async Task<User?> Validate(string email, string password)
        {
            var user = await userRepository.GetUserByEmail(email);
            if (user == null)
                return null;

            var passwordHash = new PasswordHasher<User>();
            var verificationResult = passwordHash.VerifyHashedPassword(user, user.Password, password);

            return verificationResult == PasswordVerificationResult.Success ? user : null;
        }

        /// <summary>
        /// Creates a new user in the system.
        /// </summary>
        /// <param name="user">The user object to create.</param>
        /// <returns>The created user if successful; otherwise, null.</returns>
        public async Task<User?> Create(User user)
        {
            var existingUser = await userRepository.GetUserByEmail(user.Email);

            if(existingUser != null)
                return null;

            var passwordHash = new PasswordHasher<User>();
            user.Password = passwordHash.HashPassword(user, user.Password);

            return await userRepository.Create(user);
        }

        /// <summary>
        /// Deletes a user by their ID.
        /// </summary>
        /// <param name="userId">The ID of the user to delete.</param>
        /// <returns>The deleted user if found; otherwise, null.</returns>
        public async Task<User?> Delete(int userId)
        {
            var user = await userRepository.GetUserById(userId);

            if (user == null)
                return null;

            return await userRepository.Delete(userId);
        }

        /// <summary>
        /// Updates an existing user's information.
        /// </summary>
        /// <param name="user">The updated user object.</param>
        /// <param name="userId">The ID of the user to update.</param>
        /// <returns>The updated user if found; otherwise, null.</returns>
        public async Task<User?> Update(User user, int userId)
        {
            var existingUser = await userRepository.GetUserById(userId);

            if (existingUser == null)
                return null;

            if(!string.IsNullOrEmpty(user.Password))
            {
                var passwordHash = new PasswordHasher<User>();
                user.Password = passwordHash.HashPassword(user, user.Password);
            }

            return await userRepository.Update(user);
        }
    }
}
