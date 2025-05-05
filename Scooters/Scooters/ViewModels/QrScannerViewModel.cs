using Application.Common.Interfaces;
using Application.Scooters.Queries.GetScooterById;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using Scooters.Common.Interfaces;
using ZXing.Net.Maui;

namespace Scooters.ViewModels;

public partial class QrScannerViewModel(INavigationService navigationService, IMediator mediator) : ObservableObject
{
    [ObservableProperty] private bool _isDetecting;
    [ObservableProperty] private bool _isEnabled;

    private IQrUpdaterService? _qrUpdaterService;

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
        var response = await mediator.Send(new GetScooterQuery(barcodeGuid));
        if (response.IsSuccessful)
        {
            if (response.Data is null)
            {
                await Shell.Current.DisplayAlert("QR Scanner", "There is no scooter with this id", "OK");
                return;
            }

            await navigationService.NavigateToReservationPageAsync(response.Data);
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