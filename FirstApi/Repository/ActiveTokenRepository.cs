using FirstApi.Models;
using Microsoft.EntityFrameworkCore;
using FirstApi.Interfaces;

namespace FirstApi.Repository;
public class ActiveTokenRepository : IActiveTokenRepository
{
    private MyDbContext _context { get; set; }
    public ActiveTokenRepository(MyDbContext myDbContext)
    {
        _context = myDbContext;
    }
   
    public async Task<ActiveToken> AddToActiveTokens(ActiveToken activeToken)
    {
        try
        {
            _context.ActiveTokens.Add(activeToken);
            await _context.SaveChangesAsync();
            return activeToken;
        }
        catch (DbUpdateException ex)
        {
            throw new Exception("Failed to add token to black list", ex);
        }
    }

    public async Task<ActiveToken> GetActiveTokenByUserIdAsync(int userId)
    {
        ActiveToken? activeToken = await _context.ActiveTokens.FirstOrDefaultAsync(l => l.UserId == userId);
        if (activeToken == null)
        {
            // throw new KeyNotFoundException($"ActiveToken with id {userId} not found.");
        }
        return activeToken;                            
    }

    public async Task DeleteActiveTokenAsync(int activeTokenId)
    {
        ActiveToken? activeToken = await _context.ActiveTokens.FirstOrDefaultAsync(l => l.ActiveTokenId == activeTokenId);
        if (activeToken == null)
        {
            throw new KeyNotFoundException($"ActiveToken with activeTokenId {activeTokenId} not found.");
        }
        _context.ActiveTokens.Remove(activeToken);
        await _context.SaveChangesAsync();
    }
    

}