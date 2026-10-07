using System.Text;
using Catalogo.Domain;

namespace Catalogo.Rendering;

/// <summary>
/// Convierte una página del sitio en un documento HTML completo y autocontenido
/// (el tema se inyecta como variables CSS). Es el corazón del generador estático.
/// </summary>
public sealed class PageRenderer(IEnumerable<IBlockRenderer> renderers)
{
    private readonly IReadOnlyList<IBlockRenderer> _renderers = renderers.ToList();

    /// <summary>Instancia con los renderers incluidos, útil para scripts y pruebas sin DI.</summary>
    public static PageRenderer CreateDefault() =>
        new([new HeroBlockRenderer(), new ProductGridBlockRenderer(), new FooterBlockRenderer()]);

    public string Render(Site site, Page page)
    {
        var context = new RenderContext(site, page);

        var body = new StringBuilder();
        foreach (var block in page.Blocks)
        {
            var renderer = _renderers.FirstOrDefault(r => r.CanRender(block))
                ?? throw new NotSupportedException(
                    $"No hay un renderer registrado para el bloque '{block.GetType().Name}'.");

            body.AppendLine(renderer.Render(block, context));
        }

        var title = $"{page.Title} · {site.Name}";
        var themeCss = ThemeCssBuilder.Build(site.Theme);

        return $$"""
            <!DOCTYPE html>
            <html lang="{{Html.Encode(site.Language)}}">
            <head>
            <meta charset="utf-8">
            <meta name="viewport" content="width=device-width, initial-scale=1">
            <title>{{Html.Encode(title)}}</title>
            <style>
            {{themeCss}}
            {{BaseStyles.Css}}
            </style>
            </head>
            <body>
            {{body.ToString().TrimEnd()}}
            </body>
            </html>
            """;
    }
}
