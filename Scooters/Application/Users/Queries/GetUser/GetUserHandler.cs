namespace Application.Users.Queries.GetUser;

public class GetUserHandler : IRequestHandler<GetUserQuery, ResponseData<User>>
{
    private readonly IUserRepository _userRepository;

    public GetUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ResponseData<User>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserAsync(request.Id);
        if (user is null)
        {
            return ResponseData<User>.Failure("User not found");
        }
        return ResponseData<User>.Success(user);
    }
}