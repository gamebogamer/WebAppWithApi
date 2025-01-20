using FirstApi.Interfaces;
using FirstApi.Models;
using FirstApi.Mappers;
using FirstApi.DTOs;

namespace FirstApi.Services
{
    public class UsersServices : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UsersServices(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
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
    }
}