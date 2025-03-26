namespace Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User> GetUserByIdAsync(Guid id);
    Task CreateUserAsync(User user);
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(Guid userId);
}