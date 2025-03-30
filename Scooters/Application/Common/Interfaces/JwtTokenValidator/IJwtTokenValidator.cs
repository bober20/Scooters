namespace Application.Common.Interfaces.JwtTokenValidator;

public interface IJwtTokenValidator
{
    User? ValidateToken(string token);
}