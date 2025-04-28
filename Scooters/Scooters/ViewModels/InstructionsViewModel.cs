using Application.Common.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Scooters.ViewModels;

public partial class InstructionsViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;

    public InstructionsViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }
    
    [RelayCommand]
    private void Close()
    {
        _navigationService.ClosePopupAsync();
    }
}