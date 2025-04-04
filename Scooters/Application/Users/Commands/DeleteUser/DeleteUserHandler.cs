namespace Application.Users.Commands.DeleteUser;

public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, ResponseData<bool>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    
    public DeleteUserHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    public async Task<ResponseData<bool>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserAsync(request.Id);
        if (user is null)
        {
            return ResponseData<bool>.Failure("User not found");
        }
        if (user.IsCorrectPasswordHash(request.Password, _passwordHasher))
        {
            await _userRepository.DeleteUserAsync(request.Id);
            await _unitOfWork.SaveChangesAsync();
            return ResponseData<bool>.Success(true);
        }
        return ResponseData<bool>.Failure("Password is incorrect");
    }
}