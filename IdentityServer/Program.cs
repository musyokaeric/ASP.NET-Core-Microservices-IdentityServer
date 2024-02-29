using IdentityServer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// IdentityServer4 (IS4) Registrations


// IdentityServer
builder.Services.AddIdentityServer()
    .AddInMemoryClients(Config.Clients) // Clients - Clients that need to access the API resources are defined here
    .AddInMemoryIdentityResources(Config.IdentityResources) // IdentityResource - includes user information (user id, email etc.) and can assign claim types linked to them
    // .AddInMemoryApiResources(Config.ApiResources) // ApiResource - resource (data source, web service etc.) in your system that you want to protect
    .AddInMemoryApiScopes(Config.ApiScopes) // ApiScope - represents what the client application is allowed to do
    .AddTestUsers(Config.TestUsers) // TestUser - users that will use the client application needed to access the APIs
    .AddDeveloperSigningCredential(); // Creates temporary key material at program startup

// To confirm it's working, open the link below on a browser or postman:
// https://localhost:6005/.well-known/openid-configuration

var app = builder.Build();

app.UseStaticFiles();

// IdentityServer
app.UseIdentityServer();

app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();
