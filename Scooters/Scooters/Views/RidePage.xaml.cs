using CommunityToolkit.Maui.Core;
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

public partial class RidePage : ContentPage, IBottomSheetService
{
    BottomSheetView? _bottomSheet;

    public RidePage(RideViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        viewModel.SetBottomSheetService(this);
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