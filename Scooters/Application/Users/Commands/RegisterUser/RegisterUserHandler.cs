using Domain.Abstractions;

namespace Application.Users.Commands.RegisterUser;

public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, ResponseData<Guid>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    
    public RegisterUserHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }
    
    public async Task<ResponseData<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (await _userRepository.ExistsByEmailAsync(request.Email))
            {
                return ResponseData<Guid>.Failure("Email already exists");
            }

            if (string.Equals(request.Password, request.PasswordConfirmation))
            {
                return ResponseData<Guid>.Failure("Password and confirmation password do not match");
            }
            
            var passwordHashResult = _passwordHasher.HashPassword(request.Password);

            if (!passwordHashResult.IsSuccessful)
            {
                return ResponseData<Guid>.Failure(passwordHashResult.ErrorMessage);
            }
            
            var user = new User(request.Email, passwordHashResult.Data);
            
            var newUserGuid = await _userRepository.RegisterUserAsync(user);
            await _unitOfWork.SaveChangesAsync();
            
            return ResponseData<Guid>.Success(newUserGuid);
        }
        catch(Exception ex)
        {
            return ResponseData<Guid>.Failure(ex.Message);
        }
    }
}