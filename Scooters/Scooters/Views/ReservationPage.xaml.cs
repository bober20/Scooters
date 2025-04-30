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
    BottomSheetView? bottomSheet;
    BottomSheetView? minutesBottomSheet;

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
        bottomSheet?.CloseBottomSheet();
    }

    public void ShowBottomSheetCall()
    {
#if IOS || MACCATALYST
        bottomSheet = this.ShowBottomSheet(GetBottomSheetView(), true, true);
#elif ANDROID
        bottomSheet = this.ShowBottomSheet(GetBottomSheetView(), true);
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
        minutesBottomSheet?.CloseBottomSheet();
    }

    public void ShowMinutesBottomSheetCall()
    {
        CloseBottomSheetCall();
        minutesBottomSheet = this.ShowBottomSheet(GetMinutesBottomSheetView(), true);
    }
}