using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Scooters.Common.Interfaces;
using Scooters.ViewModels;

#if ANDROID
using BottomSheetView = Google.Android.Material.BottomSheet.BottomSheetDialog;

#elif IOS || MACCATALYST
using BottomSheetView = UIKit.UIViewController;

#else
using BottomSheetView = Microsoft.UI.Xaml.Controls.Primitives.Popup;
#endif

namespace Scooters.Views;

public partial class ScootersMapPage : ContentPage, IMapUpdaterService, IBottomSheetService
{
    BottomSheetView? _bottomSheet;

    public ScootersMapPage(ScootersMapViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        viewModel.SetMapService(this);
        viewModel.SetBottomSheetService(this);
#if ANDROID
        ZoomControls.IsEnabled = false;
#endif
    }

    public void SetPins(IEnumerable<Pin> pins)
    {
        ScootersMap.Pins.Clear();
        foreach (var pin in pins)
            ScootersMap.Pins.Add(pin);
    }

    public void MoveTo(Location center, double latSpan = 0.01)
    {
        ScootersMap.MoveToRegion(MapSpan.FromCenterAndRadius(center, Distance.FromKilometers(latSpan)));
    }

    public void Zoom(double zoomLevel)
    {
        Location? location = ScootersMap?.VisibleRegion?.Center;
        if (location == null)
            return;
        ScootersMap?.MoveToRegion(MapSpan.FromCenterAndRadius(location, Distance.FromKilometers(zoomLevel)));
    }

    private View GetBottomSheetView()
    {
        var view = (View)BottomSheetTemplate.CreateContent();
        view.BindingContext = BindingContext;
        return view;
    }

    public void CloseBottomSheetCall()
    {
        _bottomSheet?.CloseBottomSheet();
    }

    public void ShowBottomSheetCall()
    {
        _bottomSheet = this.ShowBottomSheet(GetBottomSheetView(), true);
    }
}