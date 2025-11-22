using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Supabase.Gotrue;

namespace UniversityManagementSystem.Client.Auth;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly Supabase.Client _supabaseClient;

    public CustomAuthStateProvider(Supabase.Client supabaseClient)
    {
        _supabaseClient = supabaseClient;
        _supabaseClient.Auth.AddStateChangedListener((sender, state) => NotifyAuthenticationStateChanged(GetAuthenticationStateAsync()));
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var session = _supabaseClient.Auth.CurrentSession;

        if (session == null || session.AccessToken == null)
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        var user = _supabaseClient.Auth.CurrentUser;
        if (user == null)
        {
             return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Email ?? ""),
            new Claim(ClaimTypes.NameIdentifier, user.Id ?? ""),
            new Claim(ClaimTypes.Email, user.Email ?? "")
        };

        if (user.UserMetadata != null)
        {
             if (user.UserMetadata.TryGetValue("role", out var roleObj))
             {
                 claims.Add(new Claim(ClaimTypes.Role, roleObj?.ToString() ?? "student"));
             }
        }

        var identity = new ClaimsIdentity(claims, "Supabase");
        return new AuthenticationState(new ClaimsPrincipal(identity));
    }
}
