using Application.Common.Interfaces;
using Application.Scooters.Queries.GetScooterById;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using ZXing.Net.Maui;

namespace Scooters.ViewModels;

public partial class QRScannerViewModel : ObservableObject
{
    [ObservableProperty] private bool _isDetecting = true;

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
            await _navigationService.ShowPopupAsync<ReservationViewModel>(
                onPresenting: viewModel => viewModel.Scooter = response.Data);
        }
        
        IsDetecting = true;
    }

    [RelayCommand]
    private void Appearing() => IsDetecting = true;
    
    [RelayCommand]
    private void Disappearing() => IsDetecting = false;
}