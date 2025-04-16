using Application.Common.Interfaces.CurrentUserProvider;

namespace Application.Services;

public class UserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly IUnitOfWork _unitOfWork;
    
    public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, 
        IJwtTokenGenerator jwtTokenGenerator, ICurrentUserProvider currentUserProvider)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _currentUserProvider = currentUserProvider;
    }
    
    public async Task<ResponseData<string>> AuthenticateUserAsync(string email, string password)
    {
        try 
        {
            var user = await _userRepository.GetUserAsync(email);
            if (user is null)
            {
                return ResponseData<string>.Failure("User not found");
            }

            if (!user.IsCorrectPasswordHash(password, _passwordHasher))
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
    
    public async Task<ResponseData<User>> GetUserById(Guid id)
    {
        var user = await _userRepository.GetUserAsync(id);
        if (user is null)
        {
            return ResponseData<User>.Failure("User not found");
        }
        return ResponseData<User>.Success(user);
    }
    
    public async Task<ResponseData<bool>> ChangePasswordAsync(Guid userId, string oldPassword, string newPassword)
    {
        var user = await _userRepository.GetUserAsync(userId);
        if (user is null)
        {
            return ResponseData<bool>.Failure("User not found");
        }
        var isPasswordValid = user.IsCorrectPasswordHash(oldPassword, _passwordHasher);
        if (!isPasswordValid)
        {
            return ResponseData<bool>.Failure("Old password is invalid");
        }
        var newPasswordHash = _passwordHasher.HashPassword(newPassword);
        if (!newPasswordHash.IsSuccessful)
        {
            return ResponseData<bool>.Failure("New password is too weak");
        }
        user.ChangePassword(newPasswordHash.Data);
        await _userRepository.UpdateUserAsync(user);
        
        await _unitOfWork.SaveChangesAsync();
        return ResponseData<bool>.Success(true);
    }

    private async Task<ResponseData<bool>> DeleteUserAsync(Guid userId, string password)
    {
        var user = await _userRepository.GetUserAsync(userId);
        if (user is null)
        {
            return ResponseData<bool>.Failure("User not found");
        }
        if (user.IsCorrectPasswordHash(password, _passwordHasher))
        {
            await _userRepository.DeleteUserAsync(userId);
            await _unitOfWork.SaveChangesAsync();
            return ResponseData<bool>.Success(true);
        }
        return ResponseData<bool>.Failure("Password is incorrect");
    }

    public async Task<ResponseData<Guid>> RegisterUserAsync(string email, string password, string passwordConfirmation)
    {
        try
        {
            if (await _userRepository.ExistsByEmailAsync(email))
            {
                return ResponseData<Guid>.Failure("Email already exists");
            }

            if (!string.Equals(password, passwordConfirmation))
            {
                return ResponseData<Guid>.Failure("Password and confirmation password do not match");
            }
            
            var passwordHashResult = _passwordHasher.HashPassword(password);

            if (!passwordHashResult.IsSuccessful)
            {
                return ResponseData<Guid>.Failure(passwordHashResult.ErrorMessage);
            }
            
            var user = new User(email, passwordHashResult.Data);
            
            var newUserGuid = await _userRepository.RegisterUserAsync(user);
            await _unitOfWork.SaveChangesAsync();
            
            return ResponseData<Guid>.Success(newUserGuid);
        }
        catch(Exception ex)
        {
            return ResponseData<Guid>.Failure(ex.Message);
        }
    }

    public async Task UpdateUser(User user)
    {
        await _userRepository.UpdateUserAsync(user);
        await _unitOfWork.SaveChangesAsync();
    }
}