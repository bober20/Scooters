namespace Application.Common.Interfaces.NavigationService;

public interface INavigationService
{
    public Task NavigateToAsync(string page, IDictionary<string, object> parameters = null);
    public Task GoBackAsync();
}