using Application.Common.Interfaces.CurrentUserProvider;

namespace Application.Users.Queries.LogOut;

public class LogOutHandler : IRequestHandler<LogOutCommand>
{
    private readonly ICurrentUserProvider _currentUserProvider;

    public LogOutHandler(ICurrentUserProvider currentUserProvider)
    {
        _currentUserProvider = currentUserProvider;
    }

    public Task Handle(LogOutCommand request, CancellationToken cancellationToken)
    {
        _currentUserProvider.RemoveCurrentUser();
        return Task.CompletedTask;
    }
}