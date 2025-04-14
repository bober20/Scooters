// using Application.Common.Interfaces.GoogleAuthService;
// using Domain.Entities;
// using Google.SignIn;
// using UIKit;
//
// namespace Scooters.Services;
//
// public partial class GoogleAuthService : IGoogleAuthService
// {
//     //Task that we'll use for return data
//     private TaskCompletionSource<User> _taskCompletionSource;
//     
//     public GoogleAuthService()
//     {
//         Google.SignIn.SignIn.SharedInstance.Scopes = new string[] { "https://www.googleapis.com/auth/userinfo.email" };
//         // Google.SignIn.SignIn.SharedInstance.ClientId = GoogleClientID;
//     }
//     
//     public async Task<User> AuthenticateAsync()
//     {
//         _taskCompletionSource = new TaskCompletionSource<User>();
//         
//         Google.SignIn.SignIn.SharedInstance.SignedIn += SharedInstance_SignedIn;
//         //Method that will prepare PresentigViewController of Authentication Browser
//         PreparePresentedViewController();
//         //Methode that launch Browser with Google auth        
//         Google.SignIn.SignIn.SharedInstance.SignInUser();
//         // we'll modify this line later
//         return await _taskCompletionSource.Task;
//     }
//
//     public Task LogoutAsync()
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task<User> GetCurrentUserAsync()
//     {
//         throw new NotImplementedException();
//     }
//     
//     private void PreparePresentedViewController()
//     {
//         var window = UIApplication.SharedApplication.KeyWindow;
//
//         var viewController = window.RootViewController;
//
//         while(viewController.PresentingViewController != null)
//             viewController = viewController.PresentingViewController; ;
//
//         SignIn.SharedInstance.PresentingViewController = viewController;
//     }
//     
//     private void SharedInstance_SignedIn(object sender, Google.SignIn.SignInDelegateEventArgs arg)
//     {
//         if (arg.Error != null)
//         {
//             _taskCompletionSource.TrySetException(new Exception($"Error - {arg.Error.LocalizedDescription} - {Convert.ToInt32(arg.Error.Code)}"));
//             return;
//         }
//
//         var token = "";
//         SignIn.SharedInstance.CurrentUser.Authentication.GetTokens((Authentication auth, NSError error) =>
//         {
//             if (error == null)
//                 token = auth.IdToken;
//             else
//             {
//                 _taskCompletionSource.TrySetException(new Exception($"Cannot get token id ERR -> {error.Code}  - {error.LocalizedDescription}"));
//                 return;
//             }
//         });
//
//         _taskCompletionSource.TrySetResult(new User
//         {
//             Id = token,
//             Email = arg.User.Profile.Email
//         });
//     }
//
//
//
// }