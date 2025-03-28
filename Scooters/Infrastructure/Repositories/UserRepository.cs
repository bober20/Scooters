using Infrastructure.Common.Persistence;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ScootersDbContext _dbContext;
    
    public UserRepository(ScootersDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<User?> GetUserByIdOrDefaultAsync(Guid id)
    {
        var user = await _dbContext.Users
            .SingleOrDefaultAsync(u => u.Id == id);
        return user;
    }
    
    public async Task<User?> GetUserByEmailOrDefaultAsync(string email)
    {
        var user = await _dbContext.Users
            .SingleOrDefaultAsync(u => u.Email == email);
        return user;
    }

    public async Task<Guid> RegisterUserAsync(User user)
    {
        var addedUser = await _dbContext.Users.AddAsync(user);
        return addedUser.Entity.Id;
    }

    public async Task<User> UpdateUserAsync(User user)
    {
        var userEntity = await _dbContext.Users
            .SingleOrDefaultAsync(u => u.Id == user.Id);
        if (userEntity is not null)
        {
            userEntity.PasswordHash = user.PasswordHash;
            return userEntity;
        }

        throw new KeyNotFoundException("User not found");
    }

    public async Task<Guid> DeleteUserAsync(Guid userId)
    {
        var userEntity = await _dbContext.Users
            .SingleOrDefaultAsync(u => u.Id == userId);
        if (userEntity is not null)
        {
            _dbContext.Users.Remove(userEntity);
            return userEntity.Id;
        }
        
        throw new KeyNotFoundException("User not found");
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _dbContext.Users.AnyAsync(u => u.Email == email);
    }
}