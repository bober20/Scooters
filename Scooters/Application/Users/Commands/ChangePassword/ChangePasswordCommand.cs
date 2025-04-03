namespace Application.Users.Commands.ChangePassword;

public record ChangePasswordCommand(
    Guid UserId, string OldPassword, string NewPassword, string NewPasswordConfirmation) 
    : IRequest<ResponseData<bool>>;