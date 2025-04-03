using System.ComponentModel.DataAnnotations;
using Application.Users.Commands.RegisterUser;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MediatR;

namespace Scooters.ViewModels;

public partial class SignUpViewModel : ObservableValidator
{
    [ObservableProperty] 
    [Required(ErrorMessage = "Field is required.")]
    [EmailAddress]
    private string _email;
    
    [ObservableProperty] private string _emailErrors;
    [ObservableProperty] private bool _emailHasErrors;
    
    [ObservableProperty] 
    [Required(ErrorMessage = "Field is required.")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    [MaxLength(30, ErrorMessage = "Password cannot exceed 30 characters.")]
    private string _password;
    
    [ObservableProperty] private string _passwordErrors;
    [ObservableProperty] private bool _passwordHasErrors;
    
    [ObservableProperty] 
    [Required(ErrorMessage = "Field is required.")]
    [MinLength(8, ErrorMessage = "Password confirmation must be at least 8 characters long.")]
    [MaxLength(30, ErrorMessage = "Password confirmation cannot exceed 30 characters.")]
    private string _passwordConfirmation;
    
    [ObservableProperty] private string _passwordConfirmErrors;
    [ObservableProperty] private bool _passwordConfirmHasErrors;
    
    [ObservableProperty] private bool _isPasswordVisible = true;
    
    private readonly IMediator _mediator;
    
    public SignUpViewModel(IMediator mediator)
    {
        _mediator = mediator;
    }

    [RelayCommand]
    private async Task SignUp()
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
        
        if (PasswordConfirmation != Password)
        {
            PasswordConfirmHasErrors = PasswordHasErrors = true;
            PasswordConfirmErrors = PasswordErrors = "Password and confirmation password do not match";
            return;
        }

        if (!HasErrors)
        {
            var response = await _mediator.Send(new RegisterUserCommand(Email, Password, PasswordConfirmation));
        
            if (!response.IsSuccessful)
            {
                await Shell.Current.DisplayAlert("Sign up error", response.ErrorMessage, "OK");
            }
            else
            {
                await Shell.Current.GoToAsync("//LoginPage");
            }
        }
    }
    
    [RelayCommand]
    private async Task LogInLink()
    {
        await Shell.Current.GoToAsync("//LoginPage");
    }
    
    [RelayCommand]
    private void TogglePasswordVisibility() => IsPasswordVisible = !IsPasswordVisible;
    
    private bool PropertyHasErrors(string propertyName)
    {
        return GetErrors(propertyName).Any();
    }
}