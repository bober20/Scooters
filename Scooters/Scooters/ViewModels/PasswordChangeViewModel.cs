using System.ComponentModel.DataAnnotations;
using Application.Common.Interfaces.CurrentUserProvider;
using Application.Common.Interfaces.NavigationService;
using Application.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Scooters.ValidatorAttributes;

namespace Scooters.ViewModels;

public partial class PasswordChangeViewModel : ObservableValidator
{
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Field is required.")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    [MaxLength(30, ErrorMessage = "Password cannot exceed 30 characters.")]
    private string _oldPassword;

    [ObservableProperty] private string _oldPasswordErrors;
    [ObservableProperty] private bool _oldPassHasErrors;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [PasswordsMatch(nameof(NewPasswordConfirmation))]
    [Required(ErrorMessage = "Field is required.")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    [MaxLength(30, ErrorMessage = "Password cannot exceed 30 characters.")]
    private string _newPassword;

    [ObservableProperty] private string _newPasswordErrors;
    [ObservableProperty] private bool _newPassHasErrors;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [PasswordsMatch(nameof(NewPassword))]
    [Required(ErrorMessage = "Field is required.")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    [MaxLength(30, ErrorMessage = "Password cannot exceed 30 characters.")]
    private string _newPasswordConfirmation;

    [ObservableProperty] private string _newPasswordConfirmationErrors;
    [ObservableProperty] private bool _newPassConfirmHasErrors;

    [ObservableProperty] private bool _isPasswordVisible = true;

    private readonly UserService _userService;
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly INavigationService _navigationService;

    public PasswordChangeViewModel(
        ICurrentUserProvider currentUserProvider,
        INavigationService navigationService, UserService userService)
    {
        _currentUserProvider = currentUserProvider;
        _navigationService = navigationService;
        _userService = userService;
    }

    [RelayCommand]
    private async Task ChangePassword()
    {
        DisplayErrors();
        if (HasErrors)
        {
            return;
        }

        var guid = _currentUserProvider.GetCurrentUser();
        if (guid is null)
        {
            await _navigationService.NavigateToAsync("//LoginPage");
            return;
        }
        var response = await _userService.ChangePasswordAsync(guid.Value, OldPassword, NewPassword);
        if (response.IsSuccessful)
        {
            await _navigationService.ShowAlertAsync("Success", "Password has been successfully changed");
            await _navigationService.NavigateToAsync("//ProfilePage");
        }
        else
        {
            await _navigationService.ShowAlertAsync("Error", response.ErrorMessage);
        }
    }

    [RelayCommand]
    private void TogglePasswordVisibility() => IsPasswordVisible = !IsPasswordVisible;

    private bool PropertyHasErrors(string propertyName) => GetErrors(propertyName).Any();

    private void DisplayErrors()
    {
        ValidateAllProperties();

        if (PropertyHasErrors(nameof(OldPassword)))
        {
            OldPassHasErrors = true;
            OldPasswordErrors = string.Join("\n", GetErrors(nameof(OldPassword))
                .Select(e => e.ErrorMessage));
        }
        else
        {
            OldPassHasErrors = false;
            OldPasswordErrors = string.Empty;
        }

        if (PropertyHasErrors(nameof(NewPassword)))
        {
            NewPassHasErrors = true;
            NewPasswordErrors = string.Join("\n", GetErrors(nameof(NewPassword))
                .Select(e => e.ErrorMessage));
        }
        else
        {
            NewPassHasErrors = false;
            NewPasswordErrors = string.Empty;
        }

        if (PropertyHasErrors(nameof(NewPasswordConfirmation)))
        {
            NewPassConfirmHasErrors = true;
            NewPasswordConfirmationErrors = string.Join("\n", GetErrors(nameof(NewPasswordConfirmation))
                .Select(e => e.ErrorMessage));
        }
        else
        {
            NewPassConfirmHasErrors = false;
            NewPasswordConfirmationErrors = string.Empty;
        }
    }
}