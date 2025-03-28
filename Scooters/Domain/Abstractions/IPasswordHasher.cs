using Domain.Models;

namespace Domain.Abstractions;

public interface IPasswordHasher
{
    ResponseData<string> HashPassword(string password);
    bool IsCorrectPassword(string password, string hash);
}