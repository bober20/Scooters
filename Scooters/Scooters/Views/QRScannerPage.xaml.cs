using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scooters.ViewModels;
using ZXing.Net.Maui;

namespace Scooters.Views;

public partial class QRScannerPage : ContentPage
{
    public QRScannerPage(QRScannerViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}