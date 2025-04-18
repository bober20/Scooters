using System.Web;
using Microsoft.Maui.Authentication;
using Microsoft.Maui.Storage;
using System.IdentityModel.Tokens.Jwt;

namespace Scooters.Services;

public class Authenticator
{
    public async Task<AuthenticationResult> AuthenticateAsync()
    {
        try
        {
            const string clientId = "com.innowise.scootersAuth";
            const string authUrl = "https://appleid.apple.com/auth/authorize";
            const string redirectUri = "https://auth.scootersapp.com/callback";
        
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["client_id"] = clientId;
            queryString["redirect_uri"] = redirectUri;
            queryString["response_type"] = "code id_token";
            queryString["scope"] = "name email";
            queryString["response_mode"] = "form_post";
        
            var loginUrl = $"{authUrl}?{queryString}";
            
            WebAuthenticatorResult authResult = await WebAuthenticator.AuthenticateAsync(
                new Uri(loginUrl),
                new Uri(redirectUri));
            
            if (authResult.Properties.TryGetValue("id_token", out var idToken))
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(idToken);
                
                var userId = jwtToken.Subject; 
                var email = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
                var name = jwtToken.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
                
                // await SecureStorage.SetAsync("id_token", idToken);
                
                return new AuthenticationResult 
                { 
                    IsAuthenticated = true, 
                    Token = idToken,
                    UserId = userId,
                    Email = email,
                    Name = name
                };
            }
            
            return new AuthenticationResult { IsAuthenticated = false };
        }
        catch (TaskCanceledException)
        {
            return new AuthenticationResult { IsAuthenticated = false };
        }
    }
}

public class AuthenticationResult
{
    public bool IsAuthenticated { get; set; }
    public string Token { get; set; }
    public string UserId { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
}