namespace Application.Common.Interfaces;

public interface ICurrentUserProvider
{
    Guid? GetCurrentUser();
    void SetCurrentUser(string token);
    void RemoveCurrentUser();
}