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

public partial class ReservationPage : ContentPage, IBottomSheetService, IMinutesSheetService
{
    BottomSheetView? _bottomSheet;
    BottomSheetView? _minutesBottomSheet;

    public ReservationPage(ReservationViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        viewModel.SetBottomSheetService(this);
        viewModel.SetMinutesBottomSheetService(this);
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
#if IOS || MACCATALYST
        _bottomSheet = this.ShowBottomSheet(GetBottomSheetView(), true, true);
#elif ANDROID
        _bottomSheet = this.ShowBottomSheet(GetBottomSheetView(), true);
#endif
    }

    private View GetMinutesBottomSheetView()
    {
        var view = (View)MinutesBottomSheetTemplate.CreateContent();
        view.BindingContext = BindingContext;
        return view;
    }

    public void CloseMinutesBottomSheetCall()
    {
        _minutesBottomSheet?.CloseBottomSheet();
    }

    public void ShowMinutesBottomSheetCall()
    {
        CloseBottomSheetCall();
        _minutesBottomSheet = this.ShowBottomSheet(GetMinutesBottomSheetView(), true);
    }
}