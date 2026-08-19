using AsharibKamal.Application.Abstractions;
using AsharibKamal.Application.Services;
using AsharibKamal.Infrastructure;
using AsharibKamal.Web.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<IPortfolioService, PortfolioService>();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddResponseCompression();
builder.Services.AddOutputCache();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseResponseCompression();
app.UseStaticFiles();
app.UseAntiforgery();
app.UseOutputCache();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
