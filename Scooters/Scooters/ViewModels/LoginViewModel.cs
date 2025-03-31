using Application.Users.Commands.RegisterUser;
using Application.Users.Queries.AuthenticateUser;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MediatR;

namespace Scooters.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    [ObservableProperty] private string _email;
    [ObservableProperty] private string _password;
    [ObservableProperty] private string _passwordConfirmation;
    [ObservableProperty] private string? _token;
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
        var token = await _mediator.Send(new AuthenticateUserQuery(Email, Password));
        
        if (token.IsSuccessful)
        {
            Token = token.Data;
        }
        else
        {
            Token = token.ErrorMessage;
        }
    }
    
    [RelayCommand]
    private async Task SignUp()
    {
        var token = await _mediator.Send(new RegisterUserCommand(Email, Password, PasswordConfirmation));
        
        if (token.IsSuccessful)
        {
            Token = "Guid: " + token.Data.ToString();
        }
        else
        {
            Token = token.ErrorMessage;
        }
    }

    [RelayCommand]
    private async Task SignUpLink()
    {
        await Shell.Current.GoToAsync("//SignUpPage");
    }
}