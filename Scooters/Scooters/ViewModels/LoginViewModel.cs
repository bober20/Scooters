using System.ComponentModel.DataAnnotations;
using Application.Common.Interfaces;
using Application.Users.Queries.AuthenticateUser;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MediatR;

namespace Scooters.ViewModels;

public partial class LoginViewModel(
    IMediator mediator,
    ICurrentUserProvider currentUserProvider,
    INavigationService navigationService)
    : ObservableValidator
{
    [ObservableProperty]
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    private string? _email;

    [ObservableProperty] private string _emailErrors = string.Empty;
    [ObservableProperty] private bool _emailHasErrors;

    [ObservableProperty]
    [Required(ErrorMessage = "Password is required.")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    [MaxLength(30, ErrorMessage = "Password cannot exceed 30 characters.")]
    private string? _password;

    [ObservableProperty] private string _passwordErrors = string.Empty;
    [ObservableProperty] private bool _passwordHasErrors;

    [ObservableProperty] private bool _isPasswordVisible = true;
    [ObservableProperty] private string _errors = string.Empty;

    [RelayCommand]
    private async Task LogInAsync()
    {
        DisplayErrors();
        if (!HasErrors)
        {
            if (Email is null || Password is null)
            {
                await Shell.Current.DisplayAlert("Log in error", "Email and password fields cannot be null", "OK");
                return;
            }

            var response = await mediator.Send(new AuthenticateUserQuery(Email, Password));

            if (response.IsSuccessful && response.Data is not null)
            {
                currentUserProvider.SetCurrentUser(response.Data);
                await navigationService.NavigateToMapPageAsync();
            }
            else
            {
                await Shell.Current.DisplayAlert("Log in error", response.ErrorMessage, "OK");
            }
        }
    }

    [RelayCommand]
    private void TogglePasswordVisibility() => IsPasswordVisible = !IsPasswordVisible;

    [RelayCommand]
    private async Task SignUpLink() => await navigationService.NavigateToSignUpPageAsync();

    [RelayCommand]
    private async Task Appearing() => await GetCurrentUserAsync();

    private bool PropertyHasErrors(string propertyName) => GetErrors(propertyName).Any();

    private async Task GetCurrentUserAsync()
    {
        var user = currentUserProvider.GetCurrentUser();
        if (user != null)
        {
            await navigationService.NavigateToMapPageAsync();
        }
    }

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
    }
}