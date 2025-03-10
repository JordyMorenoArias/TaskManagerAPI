using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagerAPI.Models;
using TaskManagerAPI.Models.User;
using TaskManagerAPI.Repositories;
using TaskManagerAPI.Services;

namespace TaskManagerAPI.Controllers
{
    /// <summary>
    /// Controller for user management.
    /// Provides functionalities for authentication, creation, updating, and deletion of users.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// UserController constructor.
        /// </summary>
        /// <param name="userService">User service.</param>
        /// <param name="configuration">Application configuration.</param>
        public UserController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        /// <summary>
        /// Authenticates a user and generates a JWT token.
        /// </summary>
        /// <param name="userLoginDTO">User authentication data.</param>
        /// <returns>JWT token if authentication is successful; otherwise, an error message.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userLoginDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var authenticatedUser = await _userService.Validate(userLoginDTO.Email, userLoginDTO.Password);

                if (authenticatedUser == null)
                    return Unauthorized(new { message = "Invalid email or password." });

                if (!authenticatedUser.IsEmailVerified)
                    return Unauthorized(new { message = "Email not verified." });

                var jwt = _configuration.GetSection("Jwt").Get<Jwt>();

                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, jwt!.Subject),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                    new Claim("Id", authenticatedUser.Id.ToString()),
                    new Claim("Email", authenticatedUser.Email)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                        jwt.Issuer,
                        jwt.Audience,
                        claims,
                        expires: DateTime.UtcNow.AddDays(1),
                        signingCredentials: signIn
                );

                return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="userCreateDTO">User data to create.</param>
        /// <returns>Created user if the operation is successful.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserCreateDTO userCreateDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var createdUser = await _userService.Create(userCreateDTO);
                return Ok(createdUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Updates the data of an authenticated user.
        /// </summary>
        /// <param name="userUpdateDTO">Updated user data.</param>
        /// <returns>Updated user if the operation is successful.</returns>
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UserUpdateDTO userUpdateDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var userId = _userService.GetAuthenticatedUserId(HttpContext);

                userUpdateDTO.Id = userId!.Value;
                var updatedUser = await _userService.Update(userUpdateDTO);
                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Deletes an authenticated user.
        /// </summary>
        /// <returns>Deletion confirmation if the operation is successful.</returns>
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete()
        {
            try
            {
                var userId = _userService.GetAuthenticatedUserId(HttpContext);

                await _userService.Delete(userId!.Value);
                return Ok("The User was eliminated ");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Verifies a user's email address.
        /// </summary>
        /// <param name="token">Verification token.</param>
        /// <returns>Verification confirmation if the operation is successful.</returns>
        [HttpGet("verify")]
        public async Task<IActionResult> Verify([FromQuery] string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                    return BadRequest(new { message = "Token is required." });

                var user = await _userService.Verify(token);

                if (user == null)
                    return BadRequest(new { message = "Invalid verification token." });

                return Ok("Email verified successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
