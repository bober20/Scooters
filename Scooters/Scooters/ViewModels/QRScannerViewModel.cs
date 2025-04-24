using Application.Common.Interfaces.NavigationService;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ZXing.Net.Maui;

namespace Scooters.ViewModels;

public partial class QRScannerViewModel : ObservableObject
{
    [ObservableProperty] private bool _isDetecting = true;

    private readonly INavigationService _navigationService;

    public QRScannerViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    [RelayCommand]
    private async Task BarcodeDetected(BarcodeDetectionEventArgs e)
    {
        IsDetecting = false;
        var barcode = e.Results.FirstOrDefault();
        if (barcode == null)
        {
            IsDetecting = true;
            return;
        }
        
        Guid.TryParse(barcode.Value, out var barcodeGuid);
        
        await _navigationService.ShowPopupAsync<ReservationViewModel>(
            onPresenting: viewModel => viewModel.ScooterId = barcodeGuid);
        
        IsDetecting = true;
    }

    [RelayCommand]
    private void Appearing()
    {
        IsDetecting = true;
    }
    
    [RelayCommand]
    private void Disappearing()
    {
        IsDetecting = false;
    }
}