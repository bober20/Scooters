using Infrastructure.Common.Persistence;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ScootersDbContext _dbContext;
    
    public UserRepository(ScootersDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        var user = await _dbContext.Users
            .SingleOrDefaultAsync(u => u.Id == id);
        return user;
    }

    public async Task CreateUserAsync(User user)
    {
        await _dbContext.Users.AddAsync(user);
    }

    public async Task UpdateUserAsync(User user)
    {
        var userEntity = await _dbContext.Users
            .SingleOrDefaultAsync(u => u.Id == user.Id);
        if (userEntity is not null)
        {
            userEntity.PasswordHash = user.PasswordHash;
        }
    }

    public async Task DeleteUserAsync(Guid userId)
    {
        var userEntity = await _dbContext.Users
            .SingleOrDefaultAsync(u => u.Id == userId);
        if (userEntity is not null)
        {
            _dbContext.Users.Remove(userEntity);
        }
    }
}