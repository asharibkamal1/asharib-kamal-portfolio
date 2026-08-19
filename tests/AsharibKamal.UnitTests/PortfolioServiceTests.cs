using AsharibKamal.Application.Services;
using Xunit;

namespace AsharibKamal.UnitTests;

public sealed class PortfolioServiceTests
{
    [Fact]
    public void FeaturedProjects_ShouldContainPortfolioProjects()
    {
        var sut = new PortfolioService();
        var projects = sut.GetFeaturedProjects();
        Assert.NotEmpty(projects);
        Assert.Contains(projects, x => x.Slug == "digital-invoicing-fbr");
    }
}
