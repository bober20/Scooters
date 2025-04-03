using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MediatR;

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
    [ObservableProperty] private bool _oldPassHasErrors = false;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Field is required.")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    [MaxLength(30, ErrorMessage = "Password cannot exceed 30 characters.")]
    private string _newPassword;

    [ObservableProperty] private string _newPasswordErrors;
    [ObservableProperty] private bool _newPassHasErrors = false;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Field is required.")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    [MaxLength(30, ErrorMessage = "Password cannot exceed 30 characters.")]
    private string _newPasswordConfirmation;

    [ObservableProperty] private string _newPasswordConfirmationErrors;
    [ObservableProperty] private bool _newPassConfirmHasErrors = false;
    
    [ObservableProperty] private bool _isPasswordVisible = true;

    private readonly IMediator _mediator;

    public PasswordChangeViewModel(IMediator mediator)
    {
        _mediator = mediator;
    }

    [RelayCommand]
    private void ChangePassword()
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

        if (NewPasswordConfirmation != NewPassword)
        {
            NewPasswordErrors = NewPasswordConfirmationErrors = "Passwords do not match.";
            NewPassConfirmHasErrors = NewPassHasErrors = true;
            return;
        } 
        
    }
    
    [RelayCommand]
    private void TogglePasswordVisibility() => IsPasswordVisible = !IsPasswordVisible;

    private bool PropertyHasErrors(string propertyName)
    {
        return GetErrors(propertyName).Any();
    }
}