namespace Application.Common.Interfaces.CurrentUserProvider;

public interface ICurrentUserProvider
{
    string GetCurrentUserAsync();
    void SetCurrentUserAsync(string token);
    void RemoveCurrentUser();
}