using FirstApi.Models;

namespace FirstApi.Interfaces
{
    public interface IActiveTokenRepository
    {
        public Task<ActiveToken> AddToActiveTokens(ActiveToken activeToken);

        public Task<ActiveToken> GetActiveTokenByUserIdAsync(int userId);

        public Task DeleteActiveTokenAsync(int id);
    }
}
