using System.Text.Json;
using Catalogo.Domain;

namespace Catalogo.Tests;

public class BlockSerializationTests
{
    private static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web);

    [Fact]
    public void Site_hace_roundtrip_conservando_el_tipo_de_cada_bloque()
    {
        var site = SampleData.CreateSite(
            new HeroBlock("Hola"),
            new ProductGridBlock("Productos", GridLayout.Cards, 4, ["p1"]),
            new FooterBlock("Pie", [new FooterLink("Inicio", "index.html")]));

        var json = JsonSerializer.Serialize(site, Options);
        var clone = JsonSerializer.Deserialize<Site>(json, Options);

        json.Should().Contain("\"type\":\"hero\"").And.Contain("\"type\":\"product-grid\"");
        clone.Should().NotBeNull();

        var blocks = clone!.Pages[0].Blocks;
        blocks.Should().HaveCount(3);
        blocks[0].Should().BeOfType<HeroBlock>().Which.Title.Should().Be("Hola");

        var grid = blocks[1].Should().BeOfType<ProductGridBlock>().Subject;
        grid.Layout.Should().Be(GridLayout.Cards);
        grid.Columns.Should().Be(4);
        grid.ProductIds.Should().Equal("p1");

        blocks[2].Should().BeOfType<FooterBlock>().Which.Links.Should().ContainSingle();
    }
}
