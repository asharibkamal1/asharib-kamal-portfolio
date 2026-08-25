using System.Security.Claims;
using AsharibKamal.Application.Abstractions;
using AsharibKamal.Application.Services;
using AsharibKamal.Infrastructure;
using AsharibKamal.Infrastructure.Persistence;
using AsharibKamal.Web.Components;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
builder.Services.AddCascadingAuthenticationState();
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "AsharibKamal.Admin";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
        options.LoginPath = "/admin/login";
    });
builder.Services.AddAuthorization();

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

    await db.Database.ExecuteSqlRawAsync("""
        IF OBJECT_ID('BlogPosts', 'U') IS NOT NULL
        BEGIN
            IF COL_LENGTH('BlogPosts', 'CoverImageUrl') IS NULL ALTER TABLE BlogPosts ADD CoverImageUrl nvarchar(1000) NULL;
            IF COL_LENGTH('BlogPosts', 'SeoTitle') IS NULL ALTER TABLE BlogPosts ADD SeoTitle nvarchar(70) NULL;
            IF COL_LENGTH('BlogPosts', 'SeoDescription') IS NULL ALTER TABLE BlogPosts ADD SeoDescription nvarchar(170) NULL;
        END
        IF OBJECT_ID('Projects', 'U') IS NOT NULL
        BEGIN
            IF COL_LENGTH('Projects', 'CoverImageUrl') IS NULL ALTER TABLE Projects ADD CoverImageUrl nvarchar(1000) NULL;
            IF COL_LENGTH('Projects', 'Problem') IS NULL ALTER TABLE Projects ADD Problem nvarchar(1500) NULL;
            IF COL_LENGTH('Projects', 'Role') IS NULL ALTER TABLE Projects ADD Role nvarchar(1500) NULL;
            IF COL_LENGTH('Projects', 'Engineering') IS NULL ALTER TABLE Projects ADD Engineering nvarchar(1500) NULL;
            IF COL_LENGTH('Projects', 'Confidentiality') IS NULL ALTER TABLE Projects ADD Confidentiality nvarchar(1500) NULL;
            IF COL_LENGTH('Projects', 'IsPublished') IS NULL ALTER TABLE Projects ADD IsPublished bit NOT NULL CONSTRAINT DF_Projects_IsPublished DEFAULT(0);
        END
        """);
}

app.UseHttpsRedirection();
app.UseResponseCompression();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();
app.UseOutputCache();

app.MapPost("/admin/auth/login", async (HttpContext context, IConfiguration configuration) =>
{
    var form = await context.Request.ReadFormAsync();
    var email = form["email"].ToString().Trim();
    var password = form["password"].ToString();
    var returnUrl = form["returnUrl"].ToString();
    var configuredEmail = configuration["AdminAuth:Email"] ?? Environment.GetEnvironmentVariable("PORTFOLIO_ADMIN_EMAIL");
    var configuredPassword = configuration["AdminAuth:Password"] ?? Environment.GetEnvironmentVariable("PORTFOLIO_ADMIN_PASSWORD");

    if (string.IsNullOrWhiteSpace(configuredEmail) || string.IsNullOrWhiteSpace(configuredPassword)) return Results.Redirect("/admin/login?error=not-configured");
    if (!string.Equals(email, configuredEmail, StringComparison.OrdinalIgnoreCase) || !string.Equals(password, configuredPassword, StringComparison.Ordinal)) return Results.Redirect("/admin/login?error=invalid");

    var claims = new[] { new Claim(ClaimTypes.Name, configuredEmail), new Claim(ClaimTypes.Email, configuredEmail), new Claim(ClaimTypes.Role, "Administrator") };
    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
    await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), new AuthenticationProperties { IsPersistent = true });
    var safeReturnUrl = !string.IsNullOrWhiteSpace(returnUrl) && returnUrl.StartsWith('/') && !returnUrl.StartsWith("//") ? returnUrl : "/admin";
    return Results.Redirect(safeReturnUrl);
}).AllowAnonymous().DisableAntiforgery();

app.MapPost("/admin/logout", async (HttpContext context) =>
{
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return Results.Redirect("/");
}).RequireAuthorization().DisableAntiforgery();

app.MapGet("/robots.txt", () => Results.Text("User-agent: *\nAllow: /\nDisallow: /admin\nSitemap: /sitemap.xml", "text/plain"));
app.MapGet("/sitemap.xml", () => Results.Text("""
<?xml version="1.0" encoding="UTF-8"?>
<urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">
  <url><loc>/</loc></url><url><loc>/about</loc></url><url><loc>/experience</loc></url><url><loc>/projects</loc></url><url><loc>/blog</loc></url><url><loc>/services</loc></url><url><loc>/mentoring</loc></url><url><loc>/resources</loc></url><url><loc>/products</loc></url><url><loc>/newsletter</loc></url><url><loc>/resume</loc></url><url><loc>/contact</loc></url>
</urlset>
""", "application/xml"));

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
app.Run();
