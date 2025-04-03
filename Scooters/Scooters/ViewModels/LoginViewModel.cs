using System.ComponentModel.DataAnnotations;
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
    
    public LoginViewModel(IMediator mediator)
    {
        Email = string.Empty;
        Password = string.Empty;
        
        _mediator = mediator;
    }

    [RelayCommand]
    private async Task LogIn()
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

        if (HasErrors) return;
        
        var token = await _mediator.Send(new AuthenticateUserQuery(Email, Password));
        
        if (token.IsSuccessful)
        {
            await Shell.Current.GoToAsync("//MainPage");
        }
        else
        {
            await Shell.Current.DisplayAlert("Log in error", token.ErrorMessage, "OK");
        }
    }
    
    [RelayCommand]
    private void TogglePasswordVisibility() => IsPasswordVisible = !IsPasswordVisible;

    [RelayCommand]
    private async Task SignUpLink()
    {
        await Shell.Current.GoToAsync("//SignUpPage");
    }
    
    private bool PropertyHasErrors(string propertyName)
    {
        return GetErrors(propertyName).Any();
    }
}