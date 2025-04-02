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
    [ObservableProperty] private bool _isPasswordHidden = true;
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
        var token = await _mediator.Send(new AuthenticateUserQuery(Email, Password));
        
        if (token.IsSuccessful)
        {
            await Shell.Current.GoToAsync("//MainPage");
        }
        else
        {
            Errors = token.ErrorMessage;
        }
    }

    [RelayCommand]
    private async Task SignUpLink()
    {
        await Shell.Current.GoToAsync("//SignUpPage");
    }
}