using Microsoft.Maui.Controls.Platform.Compatibility;
using Microsoft.Maui.Platform;
using Scooters.ViewModels;
using Scooters.Views;
using UIKit;

namespace Scooters;

public static partial class PageExtensions
{
    public static UIViewController ShowBottomSheet(this Page page, IView bottomSheetContent, bool dimDismiss, bool largeDetent = false)
    {
        var mauiContext = page.Handler?.MauiContext ?? throw new Exception("MauiContext is null");
        var viewController = page.ToUIViewController(mauiContext);
        var viewControllerToPresent = bottomSheetContent.ToUIViewController(mauiContext);
        var sheet = viewControllerToPresent.SheetPresentationController;
        if (sheet is not null)
        {
            if (largeDetent)
            {
                sheet.Detents = new[]
                {
                    UISheetPresentationControllerDetent.CreateLargeDetent(),
                };
            }
            else
            {
                sheet.Detents = new[]
                {
                    UISheetPresentationControllerDetent.CreateMediumDetent()
                };
            }
            sheet.LargestUndimmedDetentIdentifier = dimDismiss ? UISheetPresentationControllerDetentIdentifier.Unknown : UISheetPresentationControllerDetentIdentifier.Medium;
            sheet.PrefersScrollingExpandsWhenScrolledToEdge = false;
            sheet.PrefersEdgeAttachedInCompactHeight = true;
            sheet.WidthFollowsPreferredContentSizeWhenEdgeAttached = true;

            if (viewControllerToPresent.PresentationController is not null)
            {
                viewControllerToPresent.PresentationController.Delegate = new SheetDismissalDelegate(page);
            }
        }
        viewController.PresentViewController(viewControllerToPresent, animated: true, null);
        return viewControllerToPresent;
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