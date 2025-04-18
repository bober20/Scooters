using System.ComponentModel.DataAnnotations;
using Application.Common.Interfaces.CurrentUserProvider;
using Application.Common.Interfaces.NavigationService;
using Application.Users.Queries.AuthenticateUser;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MediatR;

namespace Scooters.ViewModels;

public partial class LoginViewModel : ObservableValidator
{
    [ObservableProperty]
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    private string _email;

    [ObservableProperty] private string _emailErrors;
    [ObservableProperty] private bool _emailHasErrors;

    [ObservableProperty]
    [Required(ErrorMessage = "Password is required.")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    [MaxLength(30, ErrorMessage = "Password cannot exceed 30 characters.")]
    private string _password;

    [ObservableProperty] private string _passwordErrors;
    [ObservableProperty] private bool _passwordHasErrors;
    
    [ObservableProperty] private bool _isPasswordVisible = true;
    [ObservableProperty] private string? _errors;

    private readonly IMediator _mediator;
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly INavigationService _navigationService;

    public LoginViewModel(IMediator mediator, ICurrentUserProvider currentUserProvider,
        INavigationService navigationService)
    {
        _mediator = mediator;
        _currentUserProvider = currentUserProvider;
        _navigationService = navigationService;
    }
    
    [RelayCommand]
    private async Task LogIn()
    {
        DisplayErrors();
        if (HasErrors)
        {
            return;
        }

        var response = await _mediator.Send(new AuthenticateUserQuery(Email, Password));

        if (response.IsSuccessful)
        {
            _currentUserProvider.SetCurrentUser(response.Data);
            await _navigationService.NavigateToMainPageAsync();
        }
        else
        {
            await _navigationService.ShowAlertAsync("Log in error", response.ErrorMessage);
        }
    }

    [RelayCommand]
    private void TogglePasswordVisibility() => IsPasswordVisible = !IsPasswordVisible;

    [RelayCommand]
    private async Task SignUpLink() => await _navigationService.NavigateToSignUpPageAsync();

    [RelayCommand]
    private async Task Appearing() =>  await GetCurrentUser();

    private bool PropertyHasErrors(string propertyName) =>  GetErrors(propertyName).Any();
    
    private async Task GetCurrentUser()
    {
        var user = _currentUserProvider.GetCurrentUser();
        if (user != null)
        {
            await _navigationService.NavigateToMainPageAsync();
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