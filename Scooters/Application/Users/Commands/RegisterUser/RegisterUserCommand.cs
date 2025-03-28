namespace Application.Users.Commands.RegisterUser;

public record RegisterUserCommand(string Email, string Password, string PasswordConfirmation) 
    : IRequest<ResponseData<Guid>>;