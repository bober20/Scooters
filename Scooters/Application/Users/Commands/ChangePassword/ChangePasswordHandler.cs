namespace Application.Users.Commands.ChangePassword;

public class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand, ResponseData<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    
    public ChangePasswordHandler(IUnitOfWork unitOfWork,
        IUserRepository userRepository,
        IPasswordHasher passwordHasher)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }
    
    public async Task<ResponseData<bool>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserAsync(request.UserId);
        if (user is null)
        {
            return ResponseData<bool>.Failure("User not found");
        }
        var isPasswordValid = user.IsCorrectPasswordHash(request.OldPassword, _passwordHasher);
        if (!isPasswordValid)
        {
            return ResponseData<bool>.Failure("Old password is invalid");
        }
        var newPasswordHash = _passwordHasher.HashPassword(request.NewPassword);
        if (!newPasswordHash.IsSuccessful)
        {
            return ResponseData<bool>.Failure("New password is too weak");
        }
        user.ChangePassword(newPasswordHash.Data);
        await _userRepository.UpdateUserAsync(user);
        
        await _unitOfWork.SaveChangesAsync();
        return ResponseData<bool>.Success(true);
    }
}