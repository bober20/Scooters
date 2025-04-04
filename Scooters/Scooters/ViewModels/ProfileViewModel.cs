using Application.Common.Interfaces.CurrentUserProvider;
using Application.Users.Commands.DeleteUser;
using Application.Users.Queries.GetUser;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Entities;
using MediatR;

namespace Scooters.ViewModels;

public partial class ProfileViewModel : ObservableObject
{
    [ObservableProperty] private User _user;
    [ObservableProperty] private bool _isDarkMode;
    
    private readonly IMediator _mediator;
    private readonly ICurrentUserProvider _currentUserProvider;

    public ProfileViewModel(IMediator mediator, ICurrentUserProvider currentUserProvider)
    {
        _mediator = mediator;
        _currentUserProvider = currentUserProvider;
    }

    [RelayCommand]
    private async Task ChangePasswordLink()
    {
        await Shell.Current.GoToAsync("PasswordChangePage");
    }

    [RelayCommand]
    private async Task UploadPhoto()
    {
        var output = await Shell.Current.DisplayActionSheet(
            "Upload Photo", "Cancel", null, "Camera", "Gallery", "Delete");
    }
    
    [RelayCommand]
    private async Task DeleteAccount()
    {
        var passwordConfirmation = await Shell.Current.DisplayPromptAsync("Confirmation",
            "Confirm your password to proceed", "Delete", "Cancel", "Password", 30);
        
        if (passwordConfirmation is null)
        {
            return;
        }
        
        var response = await _mediator.Send(new DeleteUserCommand(User.Id, passwordConfirmation));
        if (response.IsSuccessful)
        {
            _currentUserProvider.RemoveCurrentUser();
            await Shell.Current.DisplayAlert("Success", "Your account has been deleted", "OK");
            await Shell.Current.GoToAsync("//LoginPage");
        }
        else
        {
            await Shell.Current.DisplayAlert("Error", response.ErrorMessage, "OK");
        }
    }

    [RelayCommand]
    private async Task LogOut()
    {
        _currentUserProvider.RemoveCurrentUser();
        await Shell.Current.GoToAsync("//LoginPage");
    }

    [RelayCommand]
    private async Task Appearing()
    {
        await GetCurrentUser();
    }

    private async Task GetCurrentUser()
    {
        var guid = _currentUserProvider.GetCurrentUser();
        if (guid is null)
        {
            await Shell.Current.GoToAsync("//LoginPage");
            return;
        }
        
        var response = await _mediator.Send(new GetUserQuery(guid.Value));
        if (response.IsSuccessful)
        {
            User = response.Data;
        }
        else
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}