using Catalogo.Domain;
using Catalogo.Rendering;
using Microsoft.Extensions.DependencyInjection;

namespace Catalogo.Tests;

public class DependencyInjectionTests
{
    [Fact]
    public void AddCatalogoRendering_registra_todos_los_renderers()
    {
        using var provider = new ServiceCollection()
            .AddCatalogoRendering()
            .BuildServiceProvider();

        provider.GetServices<IBlockRenderer>().Should().HaveCount(3);

        var site = SampleData.CreateSite(new HeroBlock("Hola"), new ProductGridBlock("Productos"), new FooterBlock("Pie"));
        var html = provider.GetRequiredService<PageRenderer>().Render(site, site.Pages[0]);

        html.Should().Contain("<h1>Hola</h1>").And.Contain("Remera").And.Contain("<p>Pie</p>");
    }
}
