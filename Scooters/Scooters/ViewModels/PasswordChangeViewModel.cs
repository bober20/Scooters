using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Scooters.ViewModels;

public partial class PasswordChangeViewModel : ObservableObject
{
    [ObservableProperty] private string _oldPassword;
    [ObservableProperty] private string _newPassword;
    [ObservableProperty] private string _newPasswordConfirmation;
    [ObservableProperty] private string _errors;

    [RelayCommand]
    private async Task ChangePassword()
    {
        throw new NotImplementedException();
    }
}