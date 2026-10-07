using Catalogo.Domain;
using Catalogo.Rendering;

// Genera el mismo sitio de ejemplo con tres paletas distintas para ver el sistema de temas en acción.
// Uso: dotnet run --project src/Catalogo.Cli [carpeta-de-salida]

var products = new List<Product>
{
    new("p1", "Remera básica", 15000m, "ARS", "Algodón peinado, corte unisex."),
    new("p2", "Campera liviana", 48500m, "ARS", "Rompeviento con capucha."),
    new("p3", "Gorra clásica", 8200m, "ARS", "Ajustable, visera curva."),
    new("p4", "Mochila urbana", 36900m, "ARS", "20 litros, compartimento para notebook."),
    new("p5", "Medias x3", 6400m, "ARS"),
    new("p6", "Cinturón de cuero", 12900m, "ARS", "Hebilla metálica."),
};

var footer = new FooterBlock(
    "© 2026 Tienda Demo. Todos los derechos reservados.",
    [new FooterLink("Inicio", "index.html"), new FooterLink("Catálogo", "catalogo.html")]);

var pages = new List<Page>
{
    new("index", "Inicio",
    [
        new HeroBlock("Ropa que dura", "Colección otoño 2026", "Ver catálogo", "catalogo.html"),
        new ProductGridBlock("Destacados", GridLayout.Grid, 3, ["p1", "p2", "p3"]),
        footer,
    ]),
    new("catalogo", "Catálogo",
    [
        new ProductGridBlock("Todo el catálogo", GridLayout.Cards, 3),
        new ProductGridBlock("Vista en lista", GridLayout.List),
        footer,
    ]),
};

Theme[] themes =
[
    new("azul", "#2563eb"),
    new("verde", "#15803d", "#f7fdf9", "#14281d"),
    new("noche", "#f59e0b", "#111827", "#f3f4f6", RadiusPx: 14),
];

var baseSite = new Site("Tienda Demo", Theme.Default, products, pages);
var renderer = PageRenderer.CreateDefault();
var outputRoot = args.Length > 0 ? args[0] : "out";

foreach (var theme in themes)
{
    var site = baseSite with { Theme = theme };
    var themeDir = Path.Combine(outputRoot, theme.Name);
    Directory.CreateDirectory(themeDir);

    foreach (var page in site.Pages)
    {
        var path = Path.Combine(themeDir, $"{page.Slug}.html");
        File.WriteAllText(path, renderer.Render(site, page));
        Console.WriteLine($"Generado: {path}");
    }
}
