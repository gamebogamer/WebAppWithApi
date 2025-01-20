using FirstApi.Models;
using FirstApi.DTOs;

namespace FirstApi.Mappers
{
    public static class UserDtomapper{
        public static UserDto ToUserDto(this User user)
        {
            return new UserDto{
                UserId = user.UserId,
                UserName = user.UserName != null ? user.UserName : "",
                Email =  user.Email != null ? user.Email : "" ,
                Password =  user.Password != null ? user.Password : "", 
                DateOfBirth =  user.DateOfBirth != default ? user.DateOfBirth : default ,
                Gender =  user.Gender != null ? user.Gender : "" ,
                PhoneNumber =  user.PhoneNumber != null ? user.PhoneNumber : "" ,
                Address =  user.Address != null ? user.Address : "" ,
                UserType =  user.UserType != null ? user.UserType : "" ,
                Status =  user.Status != null ? user.Status : "" ,
                CreatedDate =  user.CreatedDate != default ? user.CreatedDate : default ,
                UpdateDate =  user.UpdateDate != null ? user.UpdateDate : null ,
                Hobby =  user.Hobby != null ? user.Hobby : ""                
            };
            
            // return mapper.Map<UserDtoMapper>(mapper);
        }
  
    }
}
