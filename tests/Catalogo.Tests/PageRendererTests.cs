using Catalogo.Domain;
using Catalogo.Rendering;

namespace Catalogo.Tests;

public class PageRendererTests
{
    private readonly PageRenderer _sut = PageRenderer.CreateDefault();

    private string Render(params Block[] blocks)
    {
        var site = SampleData.CreateSite(blocks);
        return _sut.Render(site, site.Pages[0]);
    }

    [Fact]
    public void Render_incluye_hero_con_titulo_y_boton()
    {
        var html = Render(new HeroBlock("Bienvenidos", "Lo mejor", "Ver catálogo", "/catalogo.html"));

        html.Should()
            .Contain("<h1>Bienvenidos</h1>")
            .And.Contain("href=\"/catalogo.html\"")
            .And.Contain("Ver catálogo");
    }

    [Fact]
    public void Render_aplica_el_tema_como_variables_css()
    {
        var html = Render(new HeroBlock("Hola"));

        html.Should().Contain(":root {").And.Contain("--color-primary: #2563eb;");
    }

    [Fact]
    public void Render_escapa_html_en_datos_del_cliente()
    {
        var html = Render(new ProductGridBlock("Productos"));

        html.Should().Contain("Gorra &lt;b&gt;").And.NotContain("Gorra <b>");
    }

    [Fact]
    public void Render_neutraliza_urls_peligrosas()
    {
        var html = Render(new HeroBlock("x", null, "Click", "javascript:alert(1)"));

        html.Should().NotContain("javascript:").And.Contain("href=\"#\"");
    }

    [Fact]
    public void Render_respeta_el_orden_de_ProductIds()
    {
        var html = Render(new ProductGridBlock("Orden", GridLayout.List, 3, ["p2", "p1"]));

        html.IndexOf("Gorra", StringComparison.Ordinal)
            .Should().BeLessThan(html.IndexOf("Remera", StringComparison.Ordinal));
    }

    [Fact]
    public void Render_ignora_ids_de_producto_inexistentes()
    {
        var html = Render(new ProductGridBlock("Parcial", GridLayout.Grid, 3, ["p1", "nope"]));

        html.Should().Contain("Remera").And.NotContain("Gorra");
    }

    [Theory]
    [InlineData("es-AR", "ARS 15.000,00")]
    [InlineData("en-US", "ARS 15,000.00")]
    public void Render_formatea_precios_segun_el_locale_del_sitio(string locale, string expected)
    {
        var site = SampleData.CreateSite(new ProductGridBlock("Precios")) with { Locale = locale };

        var html = _sut.Render(site, site.Pages[0]);

        html.Should().Contain(expected);
    }

    [Fact]
    public void Render_limita_las_columnas_entre_1_y_6()
    {
        var html = Render(new ProductGridBlock("Columnas", GridLayout.Grid, 99));

        html.Should().Contain("--columns: 6");
    }

    [Fact]
    public void Render_mantiene_el_orden_de_los_bloques()
    {
        var html = Render(new FooterBlock("Pie"), new HeroBlock("Arriba"));

        html.IndexOf("<footer class=\"site-footer\">", StringComparison.Ordinal)
            .Should().BeLessThan(html.IndexOf("<section class=\"hero\">", StringComparison.Ordinal));
    }

    [Fact]
    public void Render_falla_con_un_mensaje_claro_si_falta_el_renderer()
    {
        var site = SampleData.CreateSite(new UnknownBlock());

        Action act = () => _sut.Render(site, site.Pages[0]);

        act.Should().Throw<NotSupportedException>().WithMessage("*UnknownBlock*");
    }

    private sealed record UnknownBlock : Block;
}
