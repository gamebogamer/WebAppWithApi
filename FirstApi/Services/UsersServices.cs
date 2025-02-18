using FirstApi.Interfaces;
using FirstApi.Models;
using FirstApi.Mappers;
using FirstApi.DTOs;
using Microsoft.AspNetCore.Identity;

namespace FirstApi.Services
{
    public class UsersServices : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtTokenServices _jwtTokenServices;
        private readonly IPasswordHasher<LogInDTO> _passwordHasher;
        
        public UsersServices(IUserRepository userRepository,JwtTokenServices jwtTokenServices,IPasswordHasher<LogInDTO> passwordHasher)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _jwtTokenServices = jwtTokenServices;
            _passwordHasher = passwordHasher;
            
        }

        public async Task<UserDto> CreateUserAsync(CreateUserRequestDTO createUserRequestDTO)
        {
            if (createUserRequestDTO == null)
            {
                throw new ArgumentNullException(nameof(createUserRequestDTO));
            }

            User user = createUserRequestDTO.FormCreateUserRequestDtoToUser();
            User createdUser = await _userRepository.CreateUserAsync(user);
            UserDto userDto = createdUser.ToUserDto();

            return userDto;
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetUsersAsync();
            var usersDto = users.Select(user => user.ToUserDto()).ToList();

            return usersDto;
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Id must be greater than zero", nameof(id));
            }

            User user = await _userRepository.GetUserByIdAsync(id);
            UserDto userDto = user.ToUserDto();

            return userDto;
        }

        public async Task<UserDto> UpdateUserAsync(int id, CreateUserRequestDTO createUserRequestDTO)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Id must be greater than zero", nameof(id));
            }

            if (createUserRequestDTO == null)
            {
                throw new ArgumentNullException(nameof(createUserRequestDTO));
            }

            User user = createUserRequestDTO.FormCreateUserRequestDtoToUser();
            User updatedUser = await _userRepository.EditUserAsync(id, user);
            UserDto userDto = updatedUser.ToUserDto();

            return userDto;
        }

        public async Task<UserDto> DeleteUserAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Id must be greater than zero", nameof(id));
            }

            User deletedUser = await _userRepository.DeleteUserAsync(id);
            UserDto userDto = deletedUser.ToUserDto();

            return userDto;
        }

        public async Task<string> LogInAsync(LogInDTO logInDTO)
        {
            if (logInDTO == null)
            {
                throw new ArgumentNullException(nameof(logInDTO));
            }

            if (string.IsNullOrWhiteSpace(logInDTO.UserName) || string.IsNullOrWhiteSpace(logInDTO.Password))
            {
                throw new ArgumentException("Username and password must be provided.");
            }

            User user = await _userRepository.GetUserByEmailAsync(logInDTO.UserName);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with email {logInDTO.UserName} not found.");
            }

            var result = _passwordHasher.VerifyHashedPassword(logInDTO, user.Password, logInDTO.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            return _jwtTokenServices.GenerateJwtToken(user);

        }
    }
}