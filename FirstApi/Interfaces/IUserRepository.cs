using FirstApi.Models;
namespace FirstApi.Interfaces
{
    public interface IUserRepository
    {
        public Task<List<User>> GetUsersAsync();
        public Task<User> GetUserByIdAsync(int id);
        public Task<User> GetUserByEmailAsync(string email);
        public Task<User> CreateUserAsync(User user);
        public Task<User> EditUserAsync(int id, User user);
        public Task<User> DeleteUserAsync(int id);
        // public Task<ActiveToken> AddToActiveTokens(ActiveToken activeToken);
        // public Task<ActiveToken> GetActiveTokenByUserIdAsync(int userId);
    }
}
