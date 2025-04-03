using Application.Common.Interfaces.CurrentUserProvider;

namespace Application.Users.Queries.AuthenticateUser;

public class AuthenticateUserHandler : IRequestHandler<AuthenticateUserQuery, ResponseData<string>>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ICurrentUserProvider _currentUserProvider;

    public AuthenticateUserHandler(IUserRepository userRepository,
        IJwtTokenGenerator jwtTokenGenerator,
        IPasswordHasher passwordHasher,
        ICurrentUserProvider currentUserProvider)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _passwordHasher = passwordHasher;
        _currentUserProvider = currentUserProvider;
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

            _currentUserProvider.SetCurrentUser(token);
            
            return ResponseData<string>.Success(token);
        }
        catch (Exception ex)
        {
            return ResponseData<string>.Failure(ex.Message);
        }
    }
}