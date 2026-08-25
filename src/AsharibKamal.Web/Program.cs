using AsharibKamal.Application.Abstractions;
using AsharibKamal.Application.Services;
using AsharibKamal.Infrastructure;
using AsharibKamal.Infrastructure.Persistence;
using AsharibKamal.Web.Components;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<IPortfolioService, PortfolioService>();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddResponseCompression();
builder.Services.AddOutputCache();
builder.Services.AddAntiforgery();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error", createScopeForErrors: true);
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var dbFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<PortfolioDbContext>>();
    await using var db = await dbFactory.CreateDbContextAsync();
    await db.Database.EnsureCreatedAsync();
}

app.UseHttpsRedirection();
app.UseResponseCompression();
app.UseStaticFiles();
app.UseAntiforgery();
app.UseOutputCache();

app.MapGet("/robots.txt", () => Results.Text("User-agent: *\nAllow: /\nSitemap: /sitemap.xml", "text/plain"));
app.MapGet("/sitemap.xml", () => Results.Text("""
<?xml version="1.0" encoding="UTF-8"?>
<urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">
  <url><loc>/</loc></url>
  <url><loc>/about</loc></url>
  <url><loc>/experience</loc></url>
  <url><loc>/projects</loc></url>
  <url><loc>/blog</loc></url>
  <url><loc>/services</loc></url>
  <url><loc>/mentoring</loc></url>
  <url><loc>/resources</loc></url>
  <url><loc>/products</loc></url>
  <url><loc>/newsletter</loc></url>
  <url><loc>/resume</loc></url>
  <url><loc>/contact</loc></url>
</urlset>
""", "application/xml"));

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
