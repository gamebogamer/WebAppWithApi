using FirstApi.Interfaces;
using FirstApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstApi.Repository;
class UserRepository : IUserRepository
{
    private readonly MyDbContext _context;
    public UserRepository(MyDbContext context)
    {
        _context = context;
    }
    #region IUserRepository Members

    public async Task<List<User>> GetUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }
    
    public async Task<User> GetUserByIdAsync(int id)
    {
        User? user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with id {id} not found.");
        }
        return user;
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be null or empty.", nameof(email));

        try
        {
            var user = await _context.Users
                .AsNoTracking() // Improves performance if you don't need to update the entity
                .FirstOrDefaultAsync(u => u.Email == email);

            return user ?? throw new KeyNotFoundException($"User with email '{email}' not found.");
        }
        catch (Exception ex)
        {
            // Log the error if you have a logging mechanism
            Console.WriteLine($"Error fetching user: {ex.Message}");
            throw; // Re-throw to propagate the exception
        }
    }

    public async Task<User> CreateUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }
    public async Task<User> EditUserAsync(int id, User user)
    {
        User? existingUser = await _context.Users.FindAsync(id);
        if (existingUser == null)
        {
            throw new KeyNotFoundException($"User with id {id} not found.");
        }
        existingUser.UserName = user.UserName;
        existingUser.Email = user.Email != null ? user.Email : "";
        existingUser.Password = user.Password != null ? user.Password : "";
        existingUser.DateOfBirth = user.DateOfBirth != default ? user.DateOfBirth : default;
        existingUser.Gender = user.Gender != null ? user.Gender : "";
        existingUser.PhoneNumber = user.PhoneNumber != null ? user.PhoneNumber : "";
        existingUser.Address = user.Address != null ? user.Address : "";
        existingUser.UserType = user.UserType != null ? user.UserType : "";
        existingUser.Status = user.Status != null ? user.Status : "";
        existingUser.CreatedDate = user.CreatedDate != default ? user.CreatedDate : default;
        existingUser.UpdateDate = user.UpdateDate != null ? user.UpdateDate : null;
        existingUser.Hobby = user.Hobby != null ? user.Hobby : "";

        await _context.SaveChangesAsync();
        return existingUser;
    }

    public async Task<User> DeleteUserAsync(int id)
    {
        User? user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with id {id} not found.");
        }
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return user;
    }
    #endregion
}