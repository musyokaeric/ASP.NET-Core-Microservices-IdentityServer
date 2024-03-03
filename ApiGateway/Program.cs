using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

var authenticationProviderKey = "IdentityApiKey";

// JWT Bearer Authentication
builder.Services.AddAuthentication().AddJwtBearer(authenticationProviderKey, options =>
{
    options.Authority = "https://localhost:6005"; // IS4
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = false,
    };
});

// Inject Ocelot
builder.Services.AddOcelot();

// Ocelot Routing Configs
builder.Configuration.AddJsonFile("ocelot.json");

var app = builder.Build();

app.MapControllers();

await app.UseOcelot();

app.Run();
