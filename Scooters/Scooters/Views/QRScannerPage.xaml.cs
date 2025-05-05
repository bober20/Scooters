using Scooters.Common.Interfaces;
using Scooters.ViewModels;
using ZXing.Net.Maui.Controls;

namespace Scooters.Views;

public partial class QrScannerPage : ContentPage, IQrUpdaterService
{
    public QrScannerPage(QrScannerViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        viewModel.SetQrUpdateService(this);
    }

    public void DisconnectQrHandler()
    {
        Scanner?.Handler?.DisconnectHandler();
    }

    public void ConnectQrHandler()
    {
        Scanner = new CameraBarcodeReaderView();
    }
}