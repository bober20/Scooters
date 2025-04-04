using System.ComponentModel.DataAnnotations;
using Application.Common.Interfaces.CurrentUserProvider;
using Application.Common.Interfaces.JwtTokenValidator;
using Application.Users.Commands.RegisterUser;
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
    
    [ObservableProperty]
    private string _emailErrors;
    [ObservableProperty]
    private bool _emailHasErrors;
    
    [ObservableProperty] 
    [Required(ErrorMessage = "Password is required.")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    [MaxLength(30, ErrorMessage = "Password cannot exceed 30 characters.")]
    private string _password;
    
    [ObservableProperty]
    private string _passwordErrors;
    [ObservableProperty]
    private bool _passwordHasErrors;
    
    [ObservableProperty] private bool _isPasswordVisible = true;
    [ObservableProperty] private string? _errors;
    private IMediator _mediator;
    private readonly ICurrentUserProvider _currentUserProvider;
    
    public LoginViewModel(IMediator mediator, ICurrentUserProvider currentUserProvider)
    {
        _mediator = mediator;
        _currentUserProvider = currentUserProvider;
    }

    [RelayCommand]
    private async Task LogIn()
    {
        DisplayErrors();
        
        if (HasErrors) return;
        
        var response = await _mediator.Send(new AuthenticateUserQuery(Email, Password));
        
        if (response.IsSuccessful)
        {
            _currentUserProvider.SetCurrentUser(response.Data);
            await Shell.Current.GoToAsync("//MainPage");
        }
        else
        {
            await Shell.Current.DisplayAlert("Log in error", response.ErrorMessage, "OK");
        }
    }
    
    [RelayCommand]
    private void TogglePasswordVisibility() => IsPasswordVisible = !IsPasswordVisible;

    [RelayCommand]
    private async Task SignUpLink()
    {
        await Shell.Current.GoToAsync("//SignUpPage");
    }
    
    [RelayCommand]
    private async Task Appearing()
    {
        await GetCurrentUser();
    }

    private async Task GetCurrentUser()
    {
        var user = _currentUserProvider.GetCurrentUser();
        if (user != null)
        {
            await Shell.Current.GoToAsync("//MainPage");
        }
    }
    
    private bool PropertyHasErrors(string propertyName)
    {
        return GetErrors(propertyName).Any();
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