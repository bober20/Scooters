namespace Application.Common.Interfaces;

public interface IJwtTokenValidator
{
    Guid? ValidateToken(string token);
}