namespace Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserByIdAsync(Guid id);
    Task<Guid> CreateUserAsync(User user);
    Task<User> UpdateUserAsync(User user);
    Task<Guid> DeleteUserAsync(Guid userId);
}