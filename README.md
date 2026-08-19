# Asharib Kamal — Personal Platform

Checkpoint 1 of the professional portfolio/blog/business platform.

## Stack
- .NET 10
- ASP.NET Core Blazor Web App
- Interactive Server rendering capability
- Three.js for the 3D hero
- Modern CSS + lightweight JavaScript
- EF Core 10 + SQL Server scaffold
- Modular-monolith solution structure

## Included in this checkpoint
- Premium dark 3D homepage
- Asharib's uploaded profile photo integrated into the hero
- Responsive layout and reduced-motion support
- Portfolio project cards and case-study routes
- Career timeline
- Services/monetization section
- Blog list and article routes
- AppraisalKaro and DotNet Guru promotion
- About, Projects, Blog, Services, Contact pages
- Domain/Application/Infrastructure/Web separation
- SQL Server DbContext scaffold
- Unit-test project scaffold
- GitHub Actions workflow

## Run locally
Prerequisite: .NET 10 SDK.

```bash
dotnet restore AsharibKamal.slnx
dotnet run --project src/AsharibKamal.Web/AsharibKamal.Web.csproj
```

Then open the HTTPS URL printed by ASP.NET Core.

## Database
The first checkpoint uses in-memory portfolio content so the UI is immediately usable. A SQL Server DbContext is already scaffolded.

Set `ConnectionStrings:PortfolioDb` using User Secrets or environment variables when the CMS/database checkpoint is implemented. Do not commit production secrets.

## Next checkpoint
1. ASP.NET Core Identity + private admin
2. SQL-backed projects/blog/services
3. Contact lead form and newsletter subscriptions
4. CMS CRUD screens
5. SEO service, sitemap, OpenGraph and structured data
6. Resume download/view page
7. Analytics events and conversion tracking
8. Production deployment pipeline and Azure infrastructure
