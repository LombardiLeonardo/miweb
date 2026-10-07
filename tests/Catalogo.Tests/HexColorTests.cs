using Catalogo.Domain;

namespace Catalogo.Tests;

public class HexColorTests
{
    [Theory]
    [InlineData("#000000", "#ffffff")]
    [InlineData("#ffffff", "#000000")]
    [InlineData("#1d4ed8", "#ffffff")]
    [InlineData("#facc15", "#000000")]
    public void ContrastingText_elige_el_color_legible(string background, string expected)
    {
        HexColor.Parse(background).ContrastingText().ToString().Should().Be(expected);
    }

    [Theory]
    [InlineData("")]
    [InlineData("rojo")]
    [InlineData("#fff")]
    [InlineData("#gggggg")]
    [InlineData("2563eb")]
    public void Parse_rechaza_valores_invalidos(string value)
    {
        Action act = () => HexColor.Parse(value);

        act.Should().Throw<FormatException>();
        HexColor.TryParse(value, out _).Should().BeFalse();
    }

    [Fact]
    public void ToString_hace_roundtrip_con_Parse()
    {
        HexColor.Parse("#1d4ed8").ToString().Should().Be("#1d4ed8");
    }

    [Fact]
    public void Mix_a_mitad_de_camino_entre_negro_y_blanco_da_gris_medio()
    {
        var mixed = HexColor.Black.Mix(HexColor.White, 0.5);

        mixed.ToString().Should().Be("#808080");
    }

    [Fact]
    public void ContrastRatio_entre_negro_y_blanco_es_21()
    {
        HexColor.ContrastRatio(HexColor.Black, HexColor.White).Should().BeApproximately(21.0, 0.001);
    }
}
