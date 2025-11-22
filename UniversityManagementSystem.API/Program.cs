var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<Supabase.Client>(_ => 
    new Supabase.Client(
        builder.Configuration["Supabase:Url"], 
        builder.Configuration["Supabase:Key"],
        new Supabase.SupabaseOptions
        {
            AutoRefreshToken = true,
            AutoConnectRealtime = true
        }));

builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Supabase:JwtSecret"] ?? "MISSING_SECRET")),
        ValidateIssuer = true,
        ValidIssuer = $"{builder.Configuration["Supabase:Url"]}/auth/v1",
        ValidateAudience = true,
        ValidAudience = "authenticated"
        // RoleClaimType is not set - we manually add ClaimTypes.Role in OnTokenValidated
    };
    
    options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"[JWT] Authentication failed: {context.Exception.Message}");
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Console.WriteLine($"[JWT] Token validated for user: {context.Principal?.Identity?.Name}");
            if (context.Principal != null)
            {
                var identity = context.Principal.Identity as System.Security.Claims.ClaimsIdentity;
                
                // Remove the 'authenticated' role claim that comes from the audience
                var authenticatedRoleClaim = identity?.FindFirst(c => c.Type == System.Security.Claims.ClaimTypes.Role && c.Value == "authenticated");
                if (authenticatedRoleClaim != null)
                {
                    identity?.RemoveClaim(authenticatedRoleClaim);
                    Console.WriteLine("[JWT] Removed 'authenticated' role claim");
                }
                
                // Extract role from user_metadata
                var userMetadataClaim = context.Principal.FindFirst("user_metadata");
                if (userMetadataClaim != null)
                {
                    try
                    {
                        var userMetadata = System.Text.Json.JsonDocument.Parse(userMetadataClaim.Value);
                        if (userMetadata.RootElement.TryGetProperty("role", out var roleElement))
                        {
                            var role = roleElement.GetString();
                            if (!string.IsNullOrEmpty(role))
                            {
                                identity?.AddClaim(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, role));
                                Console.WriteLine($"[JWT] Extracted role from user_metadata: {role}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[JWT] Failed to parse user_metadata: {ex.Message}");
                    }
                }
                
                foreach (var claim in context.Principal.Claims)
                {
                    Console.WriteLine($"[JWT] Claim: {claim.Type} = {claim.Value}");
                }
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy.WithOrigins("http://localhost:5226", "https://localhost:5226")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

builder.Services.AddAuthorization();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowBlazorClient");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
