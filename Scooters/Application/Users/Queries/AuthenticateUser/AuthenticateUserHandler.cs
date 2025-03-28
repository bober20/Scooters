using Application.Interfaces.JwtTokenGenerator;
using Domain.Abstractions;

namespace Application.Users.Queries.AuthenticateUser;

public class AuthenticateUserHandler : IRequestHandler<AuthenticateUserQuery, ResponseData<string>>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IPasswordHasher _passwordHasher;

    public AuthenticateUserHandler(IUserRepository userRepository,
        IJwtTokenGenerator jwtTokenGenerator,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _passwordHasher = passwordHasher;
    }

    public async Task<ResponseData<string>> Handle(AuthenticateUserQuery request, CancellationToken cancellationToken)
    {
        try 
        {
            var user = await _userRepository.GetUserAsync(request.Email);
            if (user is null)
            {
                return ResponseData<string>.Failure("User not found");
            }

            if (!user.IsCorrectPasswordHash(request.Password, _passwordHasher))
            {
                return ResponseData<string>.Failure("User's credentials are not valid");
            }
        
            var token = _jwtTokenGenerator.GenerateToken(user);
            
            return ResponseData<string>.Success(token);
        }
        catch (Exception ex)
        {
            return ResponseData<string>.Failure(ex.Message);
        }
    }
}