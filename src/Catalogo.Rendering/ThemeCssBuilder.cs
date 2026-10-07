using System.Text.RegularExpressions;
using Catalogo.Domain;

namespace Catalogo.Rendering;

/// <summary>Traduce un <see cref="Theme"/> a variables CSS, derivando los tonos que faltan.</summary>
public static partial class ThemeCssBuilder
{
    // Solo caracteres típicos de un font-family; evita inyectar reglas CSS desde el tema.
    [GeneratedRegex(@"^[\w\s,'""-]+$")]
    private static partial Regex SafeFontFamily();

    public static string Build(Theme theme)
    {
        var primary = HexColor.Parse(theme.Primary);
        var background = HexColor.Parse(theme.Background);
        var text = HexColor.Parse(theme.Text);

        if (!SafeFontFamily().IsMatch(theme.FontFamily))
        {
            throw new ArgumentException(
                "La tipografía contiene caracteres no permitidos.", nameof(theme));
        }

        var primaryHover = primary.Mix(text, 0.15);
        var surface = background.Mix(text, 0.04);
        var border = background.Mix(text, 0.15);
        var muted = text.Mix(background, 0.35);
        var radius = Math.Clamp(theme.RadiusPx, 0, 48);

        return $$"""
            :root {
              --color-bg: {{background}};
              --color-text: {{text}};
              --color-primary: {{primary}};
              --color-primary-hover: {{primaryHover}};
              --color-on-primary: {{primary.ContrastingText()}};
              --color-surface: {{surface}};
              --color-border: {{border}};
              --color-muted: {{muted}};
              --radius: {{radius}}px;
              --font-family: {{theme.FontFamily}};
            }
            """;
    }
}
