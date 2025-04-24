namespace Application.Common.Interfaces.NavigationService;

public interface INavigationService
{
    public Task NavigateToMainPageAsync();
    public Task NavigateToLoginPageAsync();
    public Task NavigateToSignUpPageAsync();
    public Task NavigateToPasswordChangePageAsync();
    public Task NavigateToProfilePageAsync();
    public Task NavigateToMapPageAsync();
    Task ClosePopupAsync();

    public Task ShowPopupAsync<TViewModel>(Action<TViewModel> onPresenting)
        where TViewModel : System.ComponentModel.INotifyPropertyChanged;
}