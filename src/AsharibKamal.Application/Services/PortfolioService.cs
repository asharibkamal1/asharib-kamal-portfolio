using AsharibKamal.Application.Abstractions;
using AsharibKamal.Application.DTOs;

namespace AsharibKamal.Application.Services;

public sealed class PortfolioService : IPortfolioService
{
    private static readonly ProjectDto[] Projects =
    [
        new("Digital Invoicing (FBR)", "digital-invoicing-fbr", "Secure invoicing platform with tax calculations, reporting, audit logging and real-time tracking.", [".NET", "Web APIs", "SQL Server", "Angular", "CQRS"]),
        new("IRIS 2.0", "iris-2", "Modernization work on a large-scale national tax payment platform with secure integrations.", [".NET", "MVC", "Web APIs", "SQL Server", "Angular", "Microservices"]),
        new("PRA Tax System", "pra-tax-system", "Backend payment APIs and reporting features supporting multiple tax workflows.", ["C#", ".NET", "Web APIs", "SQL Server"]),
        new("CareConnect CRM", "careconnect", "CRM platform with automated ticket assignment, notifications and service integrations.", [".NET Core", "MVC", "JavaScript", "SQL Server"]),
        new("Industrial ERP", "industrial-erp", "ERP modules for finance, accounts, sales, production, purchase and reporting.", ["C#", ".NET", "SQL Server", "RDLC"])
    ];

    private static readonly ExperienceDto[] Experience =
    [
        new("2024 — Present", "Software Engineer / .NET Developer", "PRAL / FBR", "Engineering secure backend capabilities for national tax platforms.", ["REST APIs", "Security & RBAC", "Azure DevOps", "SQL Server & Oracle"]),
        new("2023 — 2024", "Deputy Manager .NET Developer", "Daewoo Express Pakistan", "Built and supported enterprise full-stack applications and APIs.", [".NET 6", "REST APIs", "Code reviews", "Production support"]),
        new("2022", "Software Developer (.NET)", "Carbon8 Private Limited", "Enhanced business and desktop applications with reporting and database integrations.", ["C#", "VB.NET", "SQL Server", "RDLC"]),
        new("2019 — 2023", "Online Instructor", "Deakin University", "Mentored students across software development and technical subjects.", ["C#", "OOP", ".NET", "SQL", "Python", "Cyber Security"]),
        new("2019 — 2022", "Software Developer (.NET)", "Cloud Support", "Developed ERP systems for industrial clients including steel and flour mills.", ["ERP", "Web Forms", "SQL Server", "Business reporting"])
    ];

    private static readonly SkillGroupDto[] Skills =
    [
        new("Backend", ["C#", "ASP.NET Core", "Web APIs", ".NET Framework", "Worker Services"]),
        new("Data", ["SQL Server", "Oracle", "Entity Framework", "ADO.NET", "Stored Procedures"]),
        new("Frontend", ["Blazor", "Angular", "Razor", "JavaScript", "HTML", "CSS"]),
        new("Engineering", ["Clean Architecture", "RBAC", "Security", "Performance", "Logging", "CI/CD"])
    ];

    private static readonly BlogPreviewDto[] Articles =
    [
        new("Clean Architecture Is Not the Goal", "clean-architecture-is-not-the-goal", "A practical look at using architecture to reduce friction instead of adding ceremony.", ".NET Architecture", "6 min"),
        new("Your API Isn't Slow Because .NET Is Slow", "api-performance-dotnet", "A production-first checklist for finding the real bottleneck behind a slow API.", "Performance", "7 min"),
        new("Production-Ready ASP.NET Core API Security", "aspnet-core-api-security", "Practical security controls I consider before calling an API production ready.", "Security", "8 min")
    ];

    public IReadOnlyList<ProjectDto> GetFeaturedProjects() => Projects;
    public IReadOnlyList<ExperienceDto> GetExperience() => Experience;
    public IReadOnlyList<SkillGroupDto> GetSkillGroups() => Skills;
    public IReadOnlyList<BlogPreviewDto> GetLatestArticles() => Articles;
}
