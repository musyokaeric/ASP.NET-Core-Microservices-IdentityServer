using IdentityServer4.Models;
using IdentityServer4.Test;

var builder = WebApplication.CreateBuilder(args);

// IdentityServer
builder.Services.AddIdentityServer()
    .AddInMemoryClients(new List<Client>()) // Clients - Clients that need to access the API resources are defined here
    .AddInMemoryIdentityResources(new List<IdentityResource>()) // IdentityResource - includes user information (user id, email etc.) and can assign claim types linked to them
    .AddInMemoryApiResources(new List<ApiResource>()) // ApiResource - resource (data source, web service etc.) in your system that you want to protect
    .AddInMemoryApiScopes(new List<ApiScope>()) // ApiScope - represents what the client application is allowed to do
    .AddTestUsers(new List<TestUser>()) // TestUser - users that will use the client application needed to access the APIs
    .AddDeveloperSigningCredential(); // Creates temporary key material at program startup

// To confirm it's working, open the link below on a browser or postman:
// https://localhost:6005/.well-known/openid-configuration

var app = builder.Build();

// IdentityServer
app.UseIdentityServer();

app.MapGet("/", () => "Hello World!");

app.Run();
