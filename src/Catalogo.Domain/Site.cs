namespace Catalogo.Domain;

public sealed record Product(
    string Id,
    string Name,
    decimal Price,
    string Currency = "ARS",
    string? Description = null,
    string? ImageUrl = null);

public sealed record Page(
    string Slug,
    string Title,
    IReadOnlyList<Block> Blocks);

/// <summary>
/// Un sitio = contenido (productos) + tema + páginas. Contenido y diseño viven separados:
/// cambiar el <see cref="Theme"/> no toca los productos ni los bloques.
/// </summary>
/// <param name="Language">Idioma del documento (atributo lang del HTML).</param>
/// <param name="Locale">Cultura para formatear números y precios (por ejemplo "es-AR" o "en-US").</param>
public sealed record Site(
    string Name,
    Theme Theme,
    IReadOnlyList<Product> Products,
    IReadOnlyList<Page> Pages,
    string Language = "es",
    string Locale = "es-AR");
