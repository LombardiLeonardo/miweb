using Catalogo.Domain;

namespace Catalogo.Tests;

internal static class SampleData
{
    public static Site CreateSite(params Block[] blocks) =>
        new(
            "Tienda Demo",
            Theme.Default,
            [
                new Product("p1", "Remera", 15000m),
                new Product("p2", "Gorra <b>", 8000m, "ARS", "Gorra de algodón"),
            ],
            [new Page("index", "Inicio", blocks)]);
}
