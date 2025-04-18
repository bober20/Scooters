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
    Task ShowAlertAsync(string title, string message, string cancel = "OK");
    Task<string?> ShowOptionsAsync(string title, string cancel, params string[] options);
    Task<string?> PromptAsync(string title, string message, string accept = "OK", string cancel = "Cancel",
        string placeholder = "", int maxLength = -1);
}