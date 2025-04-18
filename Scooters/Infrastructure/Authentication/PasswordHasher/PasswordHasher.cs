using System.Text.RegularExpressions;

namespace Infrastructure.Authentication.PasswordHasher;

public partial class PasswordHasher : IPasswordHasher
{
    public ResponseData<string> HashPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
        {
            return ResponseData<string>.Failure("Password must be at least 8 characters long");
        }
        
        return ResponseData<string>.Success(BCrypt.Net.BCrypt.EnhancedHashPassword(password));
    }

    public bool IsCorrectPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(password, hash);
    }
}