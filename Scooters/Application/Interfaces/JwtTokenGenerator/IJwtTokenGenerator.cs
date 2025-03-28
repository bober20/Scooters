namespace Application.Interfaces.JwtTokenGenerator;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}