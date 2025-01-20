using Microsoft.AspNetCore.Mvc;
using FirstApi.DTOs;

namespace FirstApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetUsers()
        {
            try
            {
                var userDtos = await _userService.GetAllUsersAsync();
                return userDtos == null || !userDtos.Any() ? NoContent() : Ok(userDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById([FromRoute]int id)
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
        public async Task<IActionResult> AddUser([FromBody] CreateUserRequestDTO createUserRequestDTO)
        {
            try
            {
                var userDto = await _userService.CreateUserAsync(createUserRequestDTO);
                return CreatedAtAction(nameof(GetUserById), new { id = userDto.UserId }, userDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditUser([FromRoute]int id, [FromBody] CreateUserRequestDTO createUserRequestDTO)
        {
            try
            {
                var userDto = await _userService.UpdateUserAsync(id, createUserRequestDTO);
                return userDto == null ? NotFound() : Ok(userDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute]int id)
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