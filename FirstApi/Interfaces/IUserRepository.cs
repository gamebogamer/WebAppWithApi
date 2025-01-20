using FirstApi.Models;
namespace FirstApi.Interfaces 
{
    public interface IUserRepository
    {
        public Task<List<User>> GetUsersAsync();
        public Task<User> GetUserByIdAsync(int id);
        public Task<User> CreateUserAsync(User user);
        public Task<User> EditUserAsync(int id, User user);
        public Task<User> DeleteUserAsync(int id);
    }    
}
