using Application.Common.Interfaces;
using Application.Users.Commands.UpdateImage;
using Application.Users.Queries.GetUser;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Entities;
using MediatR;
using Scooters.Common.Services;

namespace Scooters.ViewModels;

public partial class ProfileViewModel(
    IMediator mediator,
    ICurrentUserProvider currentUserProvider,
    INavigationService navigationService)
    : ObservableObject
{
    [ObservableProperty] private User? _user;
    [ObservableProperty] private ImageSource? _profileImage;

    [RelayCommand]
    private async Task ChangePasswordLink() => await navigationService.NavigateToPasswordChangePageAsync();

    [RelayCommand]
    private async Task ManagePicture()
    {
        if (User is null)
        {
            await navigationService.NavigateToLoginPageAsync();
            return;
        }

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
            if (result is not null)
            {
                ImageService.RemoveImage(User.ImageName);
                User.ImageName = await ImageService.SaveImageAsync(result);
                await mediator.Send(new UpdateUserCommand(User));
                ProfileImage = ImageService.GetImage(User.ImageName);
            }
        }
    }

    [RelayCommand]
    private async Task DeleteAccount()
    {
        if (User is null)
        {
            await navigationService.NavigateToLoginPageAsync();
            return;
        }

        var result =
            await Shell.Current.DisplayAlert("Delete account", "Are you sure you want to delete account?", "Yes", "No");

        if (result)
        {
            currentUserProvider.RemoveCurrentUser();
            ImageService.RemoveImage(User.ImageName);
            await Shell.Current.DisplayAlert("Success", "Your account has been deleted", "OK");
            await navigationService.NavigateToLoginPageAsync();
        }
    }

    [RelayCommand]
    private async Task LogOut()
    {
        var result = await Shell.Current.DisplayAlert("Log out", "Are you sure you want to log out?", "Yes", "No");
        if (result)
        {
            currentUserProvider.RemoveCurrentUser();
            await navigationService.NavigateToLoginPageAsync();
        }
    }

    [RelayCommand]
    private async Task Appearing()
    {
        await GetCurrentUserAsync();

        if (User is null)
        {
            await navigationService.NavigateToLoginPageAsync();
            return;
        }

        ProfileImage = ImageService.GetImage(User.ImageName);
    }

    private async Task GetCurrentUserAsync()
    {
        var guid = currentUserProvider.GetCurrentUser();
        if (guid is null)
        {
            await navigationService.NavigateToLoginPageAsync();
            return;
        }

        var response = await mediator.Send(new GetUserQuery(guid.Value));
        if (response.IsSuccessful)
        {
            User = response.Data;
        }
        else
        {
            await navigationService.NavigateToLoginPageAsync();
        }
    }
}