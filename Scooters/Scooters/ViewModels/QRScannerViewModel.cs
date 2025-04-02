using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Scooters.ViewModels;

public partial class QRScannerViewModel : ObservableObject
{
    [RelayCommand]
    private async Task BarcodeDetected()
    {
        
    }
}