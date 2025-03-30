namespace Application.Common.Interfaces.JwtTokenGenerator;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}