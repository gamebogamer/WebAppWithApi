using FirstApi.Models;
using FirstApi.DTOs;

namespace FirstApi.Mappers
{
    public static class CreateUserDtoMapper
    {
        public static User FormCreateUserRequestDtoToUser(this CreateUserRequestDTO createUserRequestDTO)
        {
            return new User
            {
                UserName = createUserRequestDTO.UserName ?? "",
                Email = createUserRequestDTO.Email ?? "",
                Password = createUserRequestDTO.Password ?? "",
                DateOfBirth = createUserRequestDTO.DateOfBirth != default
                    ? DateTime.SpecifyKind(createUserRequestDTO.DateOfBirth, DateTimeKind.Utc)//.ToUniversalTime()
                    : DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc),
                Gender = createUserRequestDTO.Gender ?? "",
                PhoneNumber = createUserRequestDTO.PhoneNumber ?? "",
                Address = createUserRequestDTO.Address ?? "",
                UserType = createUserRequestDTO.UserType ?? "",
                Status = createUserRequestDTO.Status ?? "",
                CreatedDate = createUserRequestDTO.CreatedDate != default
                    ? DateTime.SpecifyKind(createUserRequestDTO.CreatedDate, DateTimeKind.Utc)//.ToUniversalTime()
                    : DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc),
                UpdateDate = createUserRequestDTO.UpdateDate.HasValue
                    ? DateTime.SpecifyKind(createUserRequestDTO.UpdateDate.Value, DateTimeKind.Utc)//.ToUniversalTime()
                    : (DateTime?)null,
                Hobby = createUserRequestDTO.Hobby ?? ""
            };
        }
    }
} 
    