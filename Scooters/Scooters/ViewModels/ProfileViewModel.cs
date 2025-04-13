using Application.Common.Interfaces.CurrentUserProvider;
using Application.Common.Interfaces.NavigationService;
using Application.Users.Commands.DeleteUser;
using Application.Users.Commands.UpdateImage;
using Application.Users.Queries.GetUser;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Entities;
using MediatR;
using Scooters.Services;

namespace Scooters.ViewModels;

public partial class ProfileViewModel : ObservableObject
{
    [ObservableProperty] private User _user;
    [ObservableProperty] private ImageSource _profileImage;

    private readonly IMediator _mediator;
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly INavigationService _navigationService;

    public ProfileViewModel(IMediator mediator, ICurrentUserProvider currentUserProvider,
        INavigationService navigationService)
    {
        _mediator = mediator;
        _currentUserProvider = currentUserProvider;
        _navigationService = navigationService;
    }

    [RelayCommand]
    private async Task ChangePasswordLink() => await _navigationService.NavigateToAsync("PasswordChangePage");

    [RelayCommand]
    private async Task PhotoManagement()
    {
        var output = await _navigationService.ShowOptionsAsync(
            "Upload Photo", "Cancel", "Gallery", "Delete");

        if (output == "Delete")
        {
            ProfileImage = null;
            ImageService.RemoveImage(User.ImageName);
            User.DeleteImage();
        }
        else if (output == "Gallery")
        {
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Pick a photo"
            });
            if (result is null)
            {
                return;
            }

            ImageService.RemoveImage(User.ImageName);
            User.ImageName = await ImageService.SaveImageAsync(result);
            await _mediator.Send(new UpdateUserCommand(User));
            ProfileImage = ImageService.GetImage(User.ImageName);
        }
    }

    [RelayCommand]
    private async Task DeleteAccount()
    {
        var passwordConfirmation = await _navigationService.PromptAsync("Confirmation",
            "Confirm your password to proceed", "Delete", "Cancel", "Password", 
            30);

        if (passwordConfirmation is null)
        {
            return;
        }
        
        var response = await _mediator.Send(new DeleteUserCommand(User.Id, passwordConfirmation));
        if (response.IsSuccessful)
        {
            _currentUserProvider.RemoveCurrentUser();
            ImageService.RemoveImage(User.ImageName);
            await _navigationService.ShowAlertAsync("Success", "Your account has been deleted", "OK");
            await _navigationService.NavigateToAsync("//LoginPage");
        }
        else
        {
            await _navigationService.ShowAlertAsync("Error", response.ErrorMessage, "OK");
        }
    }

    [RelayCommand]
    private async Task LogOut()
    {
        _currentUserProvider.RemoveCurrentUser();
        await _navigationService.NavigateToAsync("//LoginPage");
    }

    [RelayCommand]
    private async Task Appearing()
    {
        await GetCurrentUser();
        ProfileImage = ImageService.GetImage(User.ImageName);
    }

    private async Task GetCurrentUser()
    {
        var guid = _currentUserProvider.GetCurrentUser();
        if (guid is null)
        {
            await _navigationService.NavigateToAsync("//LoginPage");
            return;
        }

        var response = await _mediator.Send(new GetUserQuery(guid.Value));
        if (response.IsSuccessful)
        {
            User = response.Data;
        }
        else
        {
            await _navigationService.NavigateToAsync("//LoginPage");
        }
    }
}