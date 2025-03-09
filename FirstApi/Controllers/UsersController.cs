using Microsoft.AspNetCore.Mvc;
using FirstApi.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using FirstApi.Services;

namespace FirstApi.Controllers
{

    // [Authorize]
    [CustomAuthorize]
    [ApiController]
    [Route("api/[controller]")]

    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly JwtTokenServices _jwtTokenServices;

        private const string JwtCookieName = "jwt";

        public UsersController(IUserService userService, JwtTokenServices jwtTokenServices)
        {
            _userService = userService;
            _jwtTokenServices = jwtTokenServices;
        }

        [HttpPost("{logInDTO}")]
        [AllowAnonymous]
        public async Task<IActionResult> LogIn([FromBody] LogInDTO logInDTO)
        {
            if (logInDTO == null)
            {
                return BadRequest("Login data must be provided.");
            }

            try
            {
                string jwtToken = await _userService.LogInAsync(logInDTO);
                if (jwtToken == null)
                {
                    return Unauthorized();
                }
                return Ok(jwtToken);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception (e.g., using ILogger)
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogOut()
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
                if (userIdClaim == null) return Unauthorized("UserId not found in token.");

                int userId = int.Parse(userIdClaim.Value);

                UserDto userDto = await _userService.GetUserByIdAsync(userId);
                if (userDto == null) return NotFound("User not found.");

                await _userService.LogOutAsync(userId);

                // string jwtToken = Request.Cookies[JwtCookieName];
                // if (jwtToken == null)
                // {
                //     return BadRequest("No JWT token found in the request.");
                // }

                // await _userService.LogOutAsync(jwtToken);
                return Ok("Logged out successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception (e.g., using ILogger)
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetUsers()
        {
            try
            {
                var userId = HttpContext.Items["UserId"] as string;
                var email = HttpContext.Items["Email"] as string;
                var IsLogIn = HttpContext.Items["IsLogIn"] as bool?;
                var userDtos = await _userService.GetAllUsersAsync();
                return userDtos == null || !userDtos.Any() ? NoContent() : Ok(userDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById([FromRoute] int id)
        {
            try
            {
                var userDto = await _userService.GetUserByIdAsync(id);
                return userDto == null ? NotFound() : Ok(userDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddUser([FromBody] CreateUserRequestDTO createUserRequestDTO)
        {
            try
            {
                var PasswordHash = new PasswordHasher<CreateUserRequestDTO>().HashPassword(createUserRequestDTO, createUserRequestDTO.Password);
                createUserRequestDTO.Password = PasswordHash;
                var userDto = await _userService.CreateUserAsync(createUserRequestDTO);
                return CreatedAtAction(nameof(GetUserById), new { id = userDto.UserId }, userDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditUser([FromRoute] int id, [FromBody] CreateUserRequestDTO createUserRequestDTO)
        {
            try
            {
                var PasswordHash = new PasswordHasher<CreateUserRequestDTO>().HashPassword(createUserRequestDTO, createUserRequestDTO.Password);
                createUserRequestDTO.Password = PasswordHash;
                var userDto = await _userService.UpdateUserAsync(id, createUserRequestDTO);
                return userDto == null ? NotFound() : Ok(userDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            try
            {
                var userDto = await _userService.DeleteUserAsync(id);
                return userDto == null ? NotFound() : Ok(userDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}