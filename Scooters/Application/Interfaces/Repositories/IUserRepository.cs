namespace Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserAsync(Guid id);
    Task<User?> GetUserAsync(string email);
    Task<Guid> RegisterUserAsync(User user);
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(Guid userId);
    Task<bool> ExistsByEmailAsync(string email);
}