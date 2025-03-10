using Microsoft.AspNetCore.Identity;
using TaskManagerAPI.Models.User;
using TaskManagerAPI.Repositories;
using TaskManagerAPI.Services.Security;

namespace TaskManagerAPI.Services
{
    /// <summary>
    /// Service for managing users.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IEmailService _emailService;
        private readonly IServiceProvider _serviceProvider;

        public UserService(IUserRepository userRepository, ITokenGenerator tokenGenerator, IEmailService emailService, IServiceProvider serviceProvider)
        {
            _userRepository = userRepository;
            _tokenGenerator = tokenGenerator;
            _emailService = emailService;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Validates a user's credentials.
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>The authenticated user if credentials are valid; otherwise, null.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the email or password is incorrect.</exception>
        public async Task<User?> Validate(string email, string password)
        {
            if(string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                throw new ArgumentNullException("Email and password are required.");

            var user = await _userRepository.GetUserByEmail(email);
            if (user == null)
                throw new KeyNotFoundException("Invalid email or password.");

            var passwordHash = new PasswordHasher<User>();
            var verificationResult = passwordHash.VerifyHashedPassword(user, user.Password, password);

            return verificationResult == PasswordVerificationResult.Success ? user : null;
        }

        /// <summary>
        /// Creates a new user in the system.
        /// </summary>
        /// <param name="user">The user object to create.</param>
        /// <returns>The created user if successful; otherwise, null.</returns>
        /// <exception cref="ArgumentException">Thrown if a user with the same email already exists.</exception>
        public async Task<User?> Create(UserCreateDTO userCreateDTO)
        {
            var existingUser = await _userRepository.GetUserByEmail(userCreateDTO.Email);

            if (existingUser != null)
                throw new ArgumentException("User with this email already exists.");

            var passwordHash = new PasswordHasher<UserCreateDTO>();
            userCreateDTO.Password = passwordHash.HashPassword(userCreateDTO, userCreateDTO.Password);

            string token = _tokenGenerator.GenerateToken();

            var user = new User
            {
                Username = userCreateDTO.Username,
                Email = userCreateDTO.Email,
                Password = userCreateDTO.Password,
                EmailVerificationToken = token,
                IsEmailVerified = false
            };

            user = await _userRepository.Create(user);

            if (user != null)
                _emailService.SendVerificationEmail(user.Email, user.EmailVerificationToken);

            return user;
        }

        /// <summary>
        /// Updates an existing user's information.
        /// </summary>
        /// <param name="userUpdateDTO">The updated user object.</param>
        /// <returns>The updated user if found; otherwise, null.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the user is not found.</exception>
        public async Task<User?> Update(UserUpdateDTO userUpdateDTO)
        {
            var existingUser = await _userRepository.GetUserById(userUpdateDTO.Id);

            if (existingUser == null)
                throw new KeyNotFoundException("User not found.");

            if (!string.IsNullOrWhiteSpace(userUpdateDTO.Password))
            {
                var passwordHasher = new PasswordHasher<UserUpdateDTO>();
                existingUser.Password = passwordHasher.HashPassword(userUpdateDTO, userUpdateDTO.Password);
            }

            existingUser.Username = userUpdateDTO.Username ?? existingUser.Username;

            return await _userRepository.Update(existingUser);
        }

        /// <summary>
        /// Deletes a user by their ID.
        /// </summary>
        /// <param name="userId">The ID of the user to delete.</param>
        /// <returns>The deleted user if found; otherwise, null.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the user is not found.</exception>
        public async Task<User?> Delete(int userId)
        {
            var existingUser = await _userRepository.GetUserById(userId);

            if (existingUser == null)
                throw new KeyNotFoundException("User not found.");

            var taskService = _serviceProvider.GetRequiredService<ITaskService>();
            await taskService.DeleteAllTasksByUserId(userId);

            return await _userRepository.Delete(userId);
        }

        /// <summary>
        /// gets a user by their ID.
        /// </summary>
        /// <param name="userId">The ID of the user to retrieve.</param>
        /// <exception cref="KeyNotFoundException">Thrown if the user is not found.</exception>
        public async Task<User?> GetUserById(int userId)
        {
            var user = await _userRepository.GetUserById(userId);

            if (user == null)
                throw new KeyNotFoundException("User not found.");

            return user;
        }

        /// <summary>
        /// Verifies the specified token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">User not found.</exception>
        public async Task<User?> Verify(string token)
        {
            var user = await _userRepository.Verify(token);

            if (user == null)
                throw new KeyNotFoundException("User not found.");

            return await _userRepository.Update(user);
        }

        /// <summary>
        /// gets the ID of the authenticated user.
        /// </summary>
        /// <param name="httpContext">The current HTTP context.</param>
        /// <returns>The authenticated user's ID if found; otherwise, null.</returns>
        public int? GetAuthenticatedUserId(HttpContext httpContext)
        {
            var userIdClaim = httpContext.User.FindFirst("Id")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                throw new UnauthorizedAccessException("Invalid token or unauthorized access.");

            return userId;
        }
    }
}
