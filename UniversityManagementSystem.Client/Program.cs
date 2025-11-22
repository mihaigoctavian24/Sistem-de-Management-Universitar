using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using UniversityManagementSystem.Client;
using MudBlazor.Services;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp =>
{
    var supabase = sp.GetRequiredService<Supabase.Client>();
    var httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5241") };
    
    // Add authorization header if user is authenticated
    var session = supabase.Auth.CurrentSession;
    if (session?.AccessToken != null)
    {
        httpClient.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", session.AccessToken);
    }
    
    return httpClient;
});

builder.Services.AddMudServices();

builder.Services.AddScoped<Supabase.Client>(sp => 
    new Supabase.Client(
        builder.Configuration["Supabase:Url"]!, 
        builder.Configuration["Supabase:Key"],
        new Supabase.SupabaseOptions
        {
            AutoRefreshToken = true,
            AutoConnectRealtime = true
        }));

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, UniversityManagementSystem.Client.Auth.CustomAuthStateProvider>();

await builder.Build().RunAsync();
