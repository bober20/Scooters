using System.ComponentModel.DataAnnotations;
using Application.Common.Interfaces.NavigationService;
using Application.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Scooters.ValidatorAttributes;

namespace Scooters.ViewModels;

public partial class SignUpViewModel : ObservableValidator
{
    [ObservableProperty] [Required(ErrorMessage = "Field is required.")] [EmailAddress]
    private string _email;

    [ObservableProperty] private string _emailErrors;
    [ObservableProperty] private bool _emailHasErrors;

    [ObservableProperty]
    [Required(ErrorMessage = "Field is required.")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    [MaxLength(30, ErrorMessage = "Password cannot exceed 30 characters.")]
    [PasswordsMatch(nameof(PasswordConfirmation))]
    private string _password;

    [ObservableProperty] private string _passwordErrors;
    [ObservableProperty] private bool _passwordHasErrors;

    [ObservableProperty]
    [Required(ErrorMessage = "Field is required.")]
    [MinLength(8, ErrorMessage = "Password confirmation must be at least 8 characters long.")]
    [MaxLength(30, ErrorMessage = "Password confirmation cannot exceed 30 characters.")]
    [PasswordsMatch(nameof(Password))]
    private string _passwordConfirmation;

    [ObservableProperty] private string _passwordConfirmErrors;
    [ObservableProperty] private bool _passwordConfirmHasErrors;

    [ObservableProperty] private bool _isPasswordVisible = true;

    private readonly UserService _userService;
    private readonly INavigationService _navigationService;

    public SignUpViewModel(INavigationService navigationService, UserService userService)
    {
        _navigationService = navigationService;
        _userService = userService;
    }

    [RelayCommand]
    private async Task SignUp()
    {
        DisplayErrors();
        if (HasErrors) return;

        var response = await _userService.RegisterUserAsync(Email, Password, PasswordConfirmation);

        if (!response.IsSuccessful)
        {
            await _navigationService.ShowAlertAsync("Sign up error", response.ErrorMessage);
        }
        else
        {
            await _navigationService.NavigateToAsync("//LoginPage");
        }
    }

    [RelayCommand]
    private async Task LogInLink() => await _navigationService.NavigateToAsync("//LoginPage");
    

    [RelayCommand]
    private void TogglePasswordVisibility() => IsPasswordVisible = !IsPasswordVisible;

    private bool PropertyHasErrors(string propertyName) => GetErrors(propertyName).Any();
    

    private void DisplayErrors()
    {
        ValidateAllProperties();

        if (PropertyHasErrors(nameof(Email)))
        {
            EmailHasErrors = true;
            EmailErrors = string.Join("\n", GetErrors(nameof(Email))
                .Select(e => e.ErrorMessage));
        }
        else
        {
            EmailHasErrors = false;
            EmailErrors = string.Empty;
        }

        if (PropertyHasErrors(nameof(Password)))
        {
            PasswordHasErrors = true;
            PasswordErrors = string.Join("\n", GetErrors(nameof(Password))
                .Select(e => e.ErrorMessage));
        }
        else
        {
            PasswordHasErrors = false;
            PasswordErrors = string.Empty;
        }

        if (PropertyHasErrors(nameof(PasswordConfirmation)))
        {
            PasswordConfirmHasErrors = true;
            PasswordConfirmErrors = string.Join("\n", GetErrors(nameof(PasswordConfirmation))
                .Select(e => e.ErrorMessage));
        }
        else
        {
            PasswordConfirmHasErrors = false;
            PasswordConfirmErrors = string.Empty;
        }
    }
}