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
    private async Task ChangePasswordLink() => await _navigationService.NavigateToPasswordChangePageAsync();

    [RelayCommand]
    private async Task PhotoManagement()
    {
        var output = await Shell.Current.DisplayActionSheet(
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
        var result =
            await Shell.Current.DisplayAlert("Delete account", "Are you sure you want to delete account?", "Yes", "No");

        if (!result)
        {
            return;
        }
        _currentUserProvider.RemoveCurrentUser();
        ImageService.RemoveImage(User.ImageName);
        await Shell.Current.DisplayAlert("Success", "Your account has been deleted", "OK");
        await _navigationService.NavigateToLoginPageAsync();
    }

    [RelayCommand]
    private async Task LogOut()
    {
        var result = await Shell.Current.DisplayAlert("Log out", "Are you sure you want to log out?", "Yes", "No");
        if (!result)
        {
            return;
        }
        _currentUserProvider.RemoveCurrentUser();
        await _navigationService.NavigateToLoginPageAsync();
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
            await _navigationService.NavigateToLoginPageAsync();
            return;
        }

        var response = await _mediator.Send(new GetUserQuery(guid.Value));
        if (response.IsSuccessful)
        {
            User = response.Data;
        }
        else
        {
            await _navigationService.NavigateToLoginPageAsync();
        }
    }
}