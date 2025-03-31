using Application.Users.Commands.RegisterUser;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MediatR;

namespace Scooters.ViewModels;

public partial class SignUpViewModel : ObservableObject
{
    [ObservableProperty] private string _email;
    [ObservableProperty] private string _password;
    [ObservableProperty] private string _passwordConfirmation;
    [ObservableProperty] private string _errors;
    
    private readonly IMediator _mediator;
    
    public SignUpViewModel(IMediator mediator)
    {
        Email = string.Empty;
        Password = string.Empty;
        PasswordConfirmation = string.Empty;
        
        _mediator = mediator;
    }

    [RelayCommand]
    private async Task SignUp()
    {
        var response = await _mediator.Send(new RegisterUserCommand(Email, Password, PasswordConfirmation));
        
        if (!response.IsSuccessful)
        {
            Errors = response.ErrorMessage!;
        }
        else
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
    
    [RelayCommand]
    private async Task LogInLink()
    {
        await Shell.Current.GoToAsync("//LoginPage");
    }
}