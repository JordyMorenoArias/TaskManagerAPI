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
    /// Controlador para la gestión de usuarios.
    /// Proporciona funcionalidades para autenticación, creación, actualización y eliminación de usuarios.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor de UserController.
        /// </summary>
        /// <param name="userService">Servicio de usuario.</param>
        /// <param name="configuration">Configuración de la aplicación.</param>
        public UserController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        /// <summary>
        /// Autentica a un usuario y genera un token JWT.
        /// </summary>
        /// <param name="userLoginDTO">Datos del usuario para autenticación.</param>
        /// <returns>Token JWT si la autenticación es exitosa; de lo contrario, un mensaje de error.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userLoginDTO)
        {
            try
            {
                var authenticatedUser = await _userService.Validate(userLoginDTO.Email, userLoginDTO.Password);

                if (authenticatedUser == null)
                    return Unauthorized(new { message = "Invalid email or password." });

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
                        expires: DateTime.UtcNow.AddHours(5),
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
        /// Crea un nuevo usuario.
        /// </summary>
        /// <param name="userCreateDTO">Datos del usuario a crear.</param>
        /// <returns>Usuario creado si la operación es exitosa.</returns>
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] UserCreateDTO userCreateDTO)
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
        /// Actualiza los datos de un usuario autenticado.
        /// </summary>
        /// <param name="userUpdateDTO">Datos actualizados del usuario.</param>
        /// <returns>Usuario actualizado si la operación es exitosa.</returns>
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UserUpdateDTO userUpdateDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var userIdClaim = HttpContext.User.FindFirst("Id")?.Value;

                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                    return Unauthorized(new { message = "Invalid token or unauthorized access." });

                userUpdateDTO.Id = userId;
                var updatedUser = await _userService.Update(userUpdateDTO);
                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Elimina un usuario autenticado.
        /// </summary>
        /// <returns>Confirmación de eliminación si la operación es exitosa.</returns>
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete()
        {
            try
            {
                var claims = HttpContext.User.Identity as ClaimsIdentity;
                var userIdClaim = claims?.FindFirst("Id");

                if (userIdClaim == null)
                    return Unauthorized(new { message = "Unauthorized." });

                await _userService.Delete(int.Parse(userIdClaim.Value));
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
