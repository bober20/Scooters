namespace Application.Users.Commands.DeleteUser;

public record DeleteUserCommand(Guid Id, string Password) : IRequest<ResponseData<bool>>;