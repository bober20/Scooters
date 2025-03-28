namespace Application.Users.Queries.AuthenticateUser;

public record AuthenticateUserQuery(string Email, string Password) : IRequest<ResponseData<string>>;