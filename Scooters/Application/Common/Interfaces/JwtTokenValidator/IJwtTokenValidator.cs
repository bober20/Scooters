namespace Application.Common.Interfaces.JwtTokenValidator;

public interface IJwtTokenValidator
{
    Guid? ValidateToken(string token);
}