namespace Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserByIdOrDefaultAsync(Guid id);
    Task<User?> GetUserByEmailOrDefaultAsync(string email);
    Task<Guid> RegisterUserAsync(User user);
    Task<User> UpdateUserAsync(User user);
    Task<Guid> DeleteUserAsync(Guid userId);
    Task<bool> ExistsByEmailAsync(string email);
}