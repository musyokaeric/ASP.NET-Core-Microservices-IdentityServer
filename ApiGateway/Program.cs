using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Inject Ocelot
builder.Services.AddOcelot();

// Ocelot Routing Configs
builder.Configuration.AddJsonFile("ocelot.json");

var app = builder.Build();

app.MapControllers();

await app.UseOcelot();

app.Run();
