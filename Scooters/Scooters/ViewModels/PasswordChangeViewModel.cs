using System.ComponentModel.DataAnnotations;
using Application.Common.Interfaces.CurrentUserProvider;
using Application.Common.Interfaces.NavigationService;
using Application.Users.Commands.ChangePassword;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MediatR;
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

    private readonly IMediator _mediator;
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly INavigationService _navigationService;

    public PasswordChangeViewModel(IMediator mediator, 
        ICurrentUserProvider currentUserProvider, 
        INavigationService navigationService)
    {
        _currentUserProvider = currentUserProvider;
        _mediator = mediator;
        _navigationService = navigationService;
    }

    [RelayCommand]
    private async Task ChangePassword()
    {
        DisplayErrors();
        if (HasErrors) return;
        
        var guid = _currentUserProvider.GetCurrentUser();
        var response = await _mediator.Send(new ChangePasswordCommand(
            guid.Value, OldPassword, NewPassword, NewPasswordConfirmation));
        if (response.IsSuccessful)
        {
            await Shell.Current.DisplayAlert(
                "Success", "Password has been successfully changed", "OK");
            await _navigationService.NavigateToAsync("//ProfilePage");
        }
        else
        {
            await Shell.Current.DisplayAlert("Error", response.ErrorMessage, "OK");
        }
    }
    
    [RelayCommand]
    private void TogglePasswordVisibility() => IsPasswordVisible = !IsPasswordVisible;

    private bool PropertyHasErrors(string propertyName)
    {
        return GetErrors(propertyName).Any();
    }

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