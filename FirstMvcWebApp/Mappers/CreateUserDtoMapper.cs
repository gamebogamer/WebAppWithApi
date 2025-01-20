using FirstMvcWebApp.DTOs;
using FirstMvcWebApp.Models;
using FirstMvcWebApp.ViewModel;

namespace FirstMvcWebApp.Mappers;
public static class CreateUserDtoMapper 
{
    public static UserModel FromSignUpViewModelToCreateUserRequestDto(this SignUpViewModel signUpViewModel)
    {
        return new UserModel
        {
            UserName = signUpViewModel.UserName != null ? signUpViewModel.UserName : "",
            Email =  signUpViewModel.Email != null ? signUpViewModel.Email : "" ,
            Password =  signUpViewModel.Password != null ? signUpViewModel.Password : "", 
            // DateOfBirth =  DateTime.UtcNow,
             DateOfBirth = signUpViewModel.DateOfBirth != default
                    ? DateTime.SpecifyKind(signUpViewModel.DateOfBirth, DateTimeKind.Utc)//.ToUniversalTime()
                    : DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc),
            // DateOfBirth =  signUpViewModel.DateOfBirth != default ? signUpViewModel.DateOfBirth : default ,
            Gender =  signUpViewModel.Gender != null ? signUpViewModel.Gender : "" ,
            PhoneNumber =  signUpViewModel.PhoneNumber != null ? signUpViewModel.PhoneNumber : "" ,
            Address =  signUpViewModel.Address != null ? signUpViewModel.Address : "" ,
            UserType =  "Normal" ,
            Status =  "Active" ,
            CreatedDate =  DateTime.UtcNow,
            UpdateDate = DateTime.UtcNow,
            Hobby =  signUpViewModel.Hobby != null ? signUpViewModel.Hobby : "" 
        }; 
    }
} 
    