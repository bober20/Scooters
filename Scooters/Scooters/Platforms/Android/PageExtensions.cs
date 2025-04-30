using Google.Android.Material.BottomSheet;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Platform;
using Scooters.Views;

namespace Scooters;

public static partial class PageExtensions
{
    public static BottomSheetDialog ShowBottomSheet(this Page page, IView bottomSheetContent, bool dimDismiss)
    {
        var bottomSheetDialog = new BottomSheetDialog(Platform.CurrentActivity?.Window?.DecorView.FindViewById(Android.Resource.Id.Content)?.RootView?.Context);
        bottomSheetDialog.SetContentView(bottomSheetContent.ToPlatform(page.Handler?.MauiContext ?? throw new Exception("MauiContext is null")));
        bottomSheetDialog.Behavior.Hideable = dimDismiss;
        bottomSheetDialog.Behavior.FitToContents = true;
        
        bottomSheetDialog.DismissEvent += (sender, args) => 
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                if (page is ReservationPage)
                {
                    await Shell.Current.Navigation.PopModalAsync();
                }
                else if (page is RidePage)
                {
                    await Shell.Current.Navigation.PopModalAsync();
                }
            });
        };
        
        bottomSheetDialog.Show();
        return bottomSheetDialog;
    }
    
    public static void CloseBottomSheet(this BottomSheetDialog bottomSheet)
    {
        bottomSheet.Dismiss();
    }
}