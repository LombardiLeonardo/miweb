using Catalogo.Domain;
using Catalogo.Rendering;

namespace Catalogo.Tests;

public class ThemeCssBuilderTests
{
    [Fact]
    public void Build_genera_variables_y_texto_legible_sobre_el_primario()
    {
        var css = ThemeCssBuilder.Build(new Theme("noche", "#f59e0b", "#111827", "#f3f4f6"));

        css.Should()
            .Contain("--color-primary: #f59e0b;")
            .And.Contain("--color-bg: #111827;")
            .And.Contain("--color-on-primary: #000000;");
    }

    [Fact]
    public void Build_limita_el_radio_a_un_rango_razonable()
    {
        var css = ThemeCssBuilder.Build(Theme.Default with { RadiusPx = 5000 });

        css.Should().Contain("--radius: 48px;");
    }

    [Fact]
    public void Build_rechaza_tipografias_que_intentan_inyectar_css()
    {
        var theme = Theme.Default with { FontFamily = "a;}body{display:none" };

        Action act = () => ThemeCssBuilder.Build(theme);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Build_rechaza_colores_invalidos()
    {
        Action act = () => ThemeCssBuilder.Build(Theme.Default with { Primary = "azul" });

        act.Should().Throw<FormatException>();
    }
}
