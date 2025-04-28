namespace Application.Common.Interfaces;

public interface INavigationService
{
    public Task NavigateToLoginPageAsync();
    public Task NavigateToSignUpPageAsync();
    public Task NavigateToPasswordChangePageAsync();
    public Task NavigateToProfilePageAsync();
    public Task NavigateToMapPageAsync();
    Task ClosePopupAsync();

    public Task ShowPopupAsync<TViewModel>(Action<TViewModel> onPresenting)
        where TViewModel : System.ComponentModel.INotifyPropertyChanged;

    public Task ShowPopupAsync<TViewModel>()
        where TViewModel : System.ComponentModel.INotifyPropertyChanged;
}