using Application.Common.Interfaces;
using Application.Scooters.Queries.GetScooterById;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using Scooters.Common.Interfaces;
using ZXing.Net.Maui;

namespace Scooters.ViewModels;

public partial class QRScannerViewModel : ObservableObject
{
    [ObservableProperty] private bool _isDetecting;
    [ObservableProperty] private bool _isEnabled;

    private IQrUpdaterService _qrUpdaterService;
    private readonly INavigationService _navigationService;
    private readonly IMediator _mediator;

    public QRScannerViewModel(INavigationService navigationService, IMediator mediator)
    {
        _navigationService = navigationService;
        _mediator = mediator;
    }

    [RelayCommand]
    private async Task BarcodeDetected(BarcodeDetectionEventArgs e)
    {
        IsDetecting = false;
        var barcode = e.Results.FirstOrDefault();
        if (barcode is null)
        {
            IsDetecting = true;
            return;
        }

        Guid.TryParse(barcode.Value, out var barcodeGuid);
        var response = await _mediator.Send(new GetScooterQuery(barcodeGuid));
        if (response.IsSuccessful)
        {
            await _navigationService.NavigateToReservationPageAsync(response.Data);
        }

        IsDetecting = true;
    }

    [RelayCommand]
    private void Appearing()
    {
        _qrUpdaterService?.ConnectQrHandler();

        IsDetecting = true;
        IsEnabled = true;
    }

    [RelayCommand]
    private void Disappearing()
    {
        _qrUpdaterService?.DisconnectQrHandler();
    }

    public void SetQrUpdateService(IQrUpdaterService service)
    {
        _qrUpdaterService = service;
    }
}