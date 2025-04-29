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
    BottomSheetView? bottomSheet;

    public ScootersMapPage(ScootersMapViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        viewModel.SetMapService(this);
        viewModel.SetBottomSheetService(this);
    }

    public void SetPins(IEnumerable<Pin> pins)
    {
        ScootersMap.Pins.Clear();
        foreach (var pin in pins)
            ScootersMap.Pins.Add(pin);
    }

    public void MoveTo(Location center, double latSpan = 0.0001, double lonSpan = 0.0001)
    {
        ScootersMap.MoveToRegion(MapSpan.FromCenterAndRadius(center, Distance.FromKilometers(latSpan)));
    }
    
    private View GetBottomSheetView()
    {
        var view = (View)BottomSheetTemplate.CreateContent();
        view.BindingContext = BindingContext;
        return view;
    }

    public void CloseBottomSheetCall()
    {
        bottomSheet?.CloseBottomSheet();
    }

    public void ShowBottomSheetCall()
    {
        this.ShowBottomSheet(GetBottomSheetView(), true);
    }
}