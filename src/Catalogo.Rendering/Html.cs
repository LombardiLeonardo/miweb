using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace Catalogo.Rendering;

/// <summary>Helpers de seguridad para no inyectar contenido del cliente sin escapar.</summary>
internal static class Html
{
    // UnicodeRanges.All conserva tildes y eñes en lugar de convertirlas a entidades numéricas.
    private static readonly HtmlEncoder Encoder = HtmlEncoder.Create(UnicodeRanges.All);

    public static string Encode(string? value) => Encoder.Encode(value ?? string.Empty);

    /// <summary>
    /// Devuelve una URL apta para href/src ya escapada. Acepta rutas relativas, http, https y mailto;
    /// cualquier otra cosa (por ejemplo "javascript:") se reemplaza por "#".
    /// </summary>
    public static string SafeUrl(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return "#";
        }

        var trimmed = url.Trim();

        var isRelative = !trimmed.Contains(':')
            && !trimmed.Contains('\\')
            && !trimmed.StartsWith("//", StringComparison.Ordinal);
        if (isRelative)
        {
            return Encode(trimmed);
        }

        if (Uri.TryCreate(trimmed, UriKind.Absolute, out var uri)
            && uri.Scheme is "http" or "https" or "mailto")
        {
            return Encode(trimmed);
        }

        return "#";
    }
}
