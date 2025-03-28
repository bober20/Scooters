using System.Text.RegularExpressions;

namespace Infrastructure.Authentication.PasswordHasher;

public partial class PasswordHasher : IPasswordHasher
{
    private static readonly Regex PasswordRegex = StrongPasswordRegex();
    
    public ResponseData<string> HashPassword(string password)
    {
        return PasswordRegex.IsMatch(password)
            ? ResponseData<string>.Success(BCrypt.Net.BCrypt.EnhancedHashPassword(password))
            : ResponseData<string>.Failure("Password is too weak.");
    }

    public bool IsCorrectPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(password, hash);
    }
    
    [GeneratedRegex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", RegexOptions.Compiled)]
    private static partial Regex StrongPasswordRegex();
}