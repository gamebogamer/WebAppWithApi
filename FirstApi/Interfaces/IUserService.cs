using FirstApi.DTOs;
public interface IUserService
{
    public Task<List<UserDto>> GetAllUsersAsync();
    public Task<UserDto> GetUserByIdAsync(int id);
    public Task<UserDto> CreateUserAsync(CreateUserRequestDTO createUserRequestDTO);
    public Task<UserDto> UpdateUserAsync(int id, CreateUserRequestDTO createUserRequestDTO);
    public Task<UserDto> DeleteUserAsync(int id);
    public Task<string> LogInAsync(LogInDTO logInDTO);
    public Task<string> LogOutAsync(int userId);
}