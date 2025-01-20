using FirstMvcWebApp.DTOs;
using FirstMvcWebApp.Models;
using FirstMvcWebApp.ViewModel;

namespace FirstMvcWebApp.Mappers;
public static class EditUserMapper 
{
    public static EditViewModel ToEditUser(this UserModel user)
    {
        return new EditViewModel
        {
            UserId = user.UserId ,
            UserName = user.UserName != null ? user.UserName : "",
            Email =  user.Email != null ? user.Email : "" ,
            Password =  user.Password != null ? user.Password : "", 
            // DateOfBirth =  DateTime.UtcNow,
             DateOfBirth = user.DateOfBirth != default
                    ? DateTime.SpecifyKind(user.DateOfBirth, DateTimeKind.Utc)//.ToUniversalTime()
                    : DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc),
            // DateOfBirth =  user.DateOfBirth != default ? user.DateOfBirth : default ,
            Gender =  user.Gender != null ? user.Gender : "" ,
            PhoneNumber =  user.PhoneNumber != null ? user.PhoneNumber : "" ,
            Address =  user.Address != null ? user.Address : "" ,
            Hobby =  user.Hobby != null ? user.Hobby : "" 
        }; 
    }

    
    public static UserDto ToUserDto(this EditViewModel editViewModel)
    {
        return new UserDto{
                UserId = editViewModel.UserId,
                UserName = editViewModel.UserName != null ? editViewModel.UserName : "",
                Email =  editViewModel.Email != null ? editViewModel.Email : "" ,
                Password =  editViewModel.Password != null ? editViewModel.Password : "", 
                DateOfBirth =  editViewModel.DateOfBirth != default ? editViewModel.DateOfBirth : DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc) ,
                Gender =  editViewModel.Gender != null ? editViewModel.Gender : "" ,
                PhoneNumber =  editViewModel.PhoneNumber != null ? editViewModel.PhoneNumber : "" ,
                Address =  editViewModel.Address != null ? editViewModel.Address : "" ,
                UserType =  "Normal" ,
                Status =  "Active" ,
                CreatedDate =   editViewModel.DateOfBirth != default
                    ? DateTime.SpecifyKind(editViewModel.DateOfBirth, DateTimeKind.Utc)//.ToUniversalTime()
                    : DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc),
                UpdateDate =   editViewModel.DateOfBirth != default
                    ? DateTime.SpecifyKind(editViewModel.DateOfBirth, DateTimeKind.Utc)//.ToUniversalTime()
                    : DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc),
                Hobby =  editViewModel.Hobby != null ? editViewModel.Hobby : ""                
        };
    }
} 
    