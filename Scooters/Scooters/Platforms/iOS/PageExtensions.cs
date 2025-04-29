using Microsoft.Maui.Platform;
using Scooters.ViewModels;
using Scooters.Views;
using UIKit;

namespace Scooters;

public static partial class PageExtensions
{
    public static void ShowBottomSheet(this Page page, IView bottomSheetContent, bool dimDismiss)
    {
        var mauiContext = page.Handler?.MauiContext ?? throw new Exception("MauiContext is null");
        var viewController = page.ToUIViewController(mauiContext);
        var viewControllerToPresent = bottomSheetContent.ToUIViewController(mauiContext);
        var sheet = viewControllerToPresent.SheetPresentationController;
        if (sheet is not null)
        {
            sheet.Detents = new[]
            {
                UISheetPresentationControllerDetent.CreateMediumDetent(),
                UISheetPresentationControllerDetent.CreateLargeDetent(),
            };
            sheet.LargestUndimmedDetentIdentifier = dimDismiss ? UISheetPresentationControllerDetentIdentifier.Unknown : UISheetPresentationControllerDetentIdentifier.Medium;
            sheet.PrefersScrollingExpandsWhenScrolledToEdge = false;
            sheet.PrefersEdgeAttachedInCompactHeight = true;
            sheet.WidthFollowsPreferredContentSizeWhenEdgeAttached = true;
            
            viewControllerToPresent.PresentationController.Delegate = new SheetDismissalDelegate(page);
        }
        viewController.PresentViewController(viewControllerToPresent, animated: true, null);
    }
    
    public static void CloseBottomSheet(this UIViewController bottomSheet)
    {
        bottomSheet.DismissViewController(true, null);
    }
    
    private class SheetDismissalDelegate : UIAdaptivePresentationControllerDelegate
    {
        private readonly Page _page;
        
        public SheetDismissalDelegate(Page page)
        {
            _page = page;
        }
        
        public override void DidDismiss(UIPresentationController presentationController)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                if (_page is ReservationPage)
                {
                    await Shell.Current.Navigation.PopModalAsync();
                }
                else if (_page is RidePage)
                {
                    await Shell.Current.Navigation.PopModalAsync();
                }
            });
        }
    }
}