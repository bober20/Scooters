namespace Application.Common.Interfaces.CurrentUserProvider;

public interface ICurrentUserProvider
{
    Guid? GetCurrentUser();
    void SetCurrentUser(string token);
    void RemoveCurrentUser();
}