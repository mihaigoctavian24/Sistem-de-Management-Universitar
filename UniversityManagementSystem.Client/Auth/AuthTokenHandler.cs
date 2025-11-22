using System.Net.Http.Headers;

namespace UniversityManagementSystem.Client.Auth;

public class AuthTokenHandler : DelegatingHandler
{
    private readonly Supabase.Client _supabase;

    public AuthTokenHandler(Supabase.Client supabase)
    {
        _supabase = supabase;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Get the current session token from Supabase
        var session = _supabase.Auth.CurrentSession;
        
        if (session?.AccessToken != null)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", session.AccessToken);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
