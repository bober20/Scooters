using System.Net;
using Application.Common.Interfaces.NavigationService;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ZXing;
using ZXing.Net.Maui;

namespace Scooters.ViewModels;

public partial class QRScannerViewModel : ObservableObject
{
    [ObservableProperty] private Result _result;
    [ObservableProperty] private bool _isDetecting = true;

    private readonly INavigationService _navigationService;
    private readonly IPopupService _popupService;

    public QRScannerViewModel(INavigationService navigationService, IPopupService popupService)
    {
        _navigationService = navigationService;
        _popupService = popupService;
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
        
        await MainThread.InvokeOnMainThreadAsync(() =>
        {
            _popupService.ShowPopupAsync<ReservationViewModel>(
                onPresenting: viewModel => viewModel.ScooterId = barcodeGuid);
        });
        
        IsDetecting = true;
    }
}