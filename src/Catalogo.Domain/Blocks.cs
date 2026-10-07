using System.Text.Json.Serialization;

namespace Catalogo.Domain;

/// <summary>
/// Un bloque es una sección configurable de una página (hero, grilla de catálogo, footer...).
/// Para agregar un tipo nuevo: derivar aquí, registrarlo con [JsonDerivedType]
/// e implementar su <c>BlockRenderer&lt;T&gt;</c> en Catalogo.Rendering.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(HeroBlock), "hero")]
[JsonDerivedType(typeof(ProductGridBlock), "product-grid")]
[JsonDerivedType(typeof(FooterBlock), "footer")]
public abstract record Block;

public sealed record HeroBlock(
    string Title,
    string? Subtitle = null,
    string? CtaText = null,
    string? CtaUrl = null,
    string? ImageUrl = null) : Block;

[JsonConverter(typeof(JsonStringEnumConverter<GridLayout>))]
public enum GridLayout
{
    Grid,
    List,
    Cards,
}

/// <param name="ProductIds">Si es null se muestran todos los productos del sitio; si no, solo esos y en ese orden.</param>
public sealed record ProductGridBlock(
    string Heading,
    GridLayout Layout = GridLayout.Grid,
    int Columns = 3,
    IReadOnlyList<string>? ProductIds = null) : Block;

public sealed record FooterLink(string Label, string Url);

public sealed record FooterBlock(
    string Text,
    IReadOnlyList<FooterLink>? Links = null) : Block;
