using Application.Users.Queries.LogOut;
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

    public ProfileViewModel(IMediator mediator)
    {
        _mediator = mediator;
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
        
    }

    [RelayCommand]
    private async Task LogOut()
    {
        await _mediator.Send(new LogOutCommand());
        await Shell.Current.GoToAsync("//LoginPage");
    }
}