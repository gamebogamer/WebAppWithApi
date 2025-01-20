using FirstMvcWebApp.DTOs;
using FirstMvcWebApp.Models;
using FirstMvcWebApp.ViewModel;

namespace FirstMvcWebApp.Interfaces;
public interface IAccountService
{
    public Task<List<UserModel>> GetAllUsers();
    public Task<UserModel> GetUserById(int id);
    // public Task<UserModel> CreateUser(SignUpViewModel signUpViewModel);
    public Task<UserModel> CreateUser(SignUpViewModel signUpViewModel);
    public Task<UserDto> UpdateUser(int id,EditViewModel editViewModel);
    public Task<bool> DeleteUser(int UserId);
}