using System.Globalization;

namespace Catalogo.Domain;

/// <summary>
/// Color sRGB validado. Evita pasar strings arbitrarios a CSS y concentra la matemática de
/// contraste (WCAG) en un solo lugar.
/// </summary>
public readonly record struct HexColor(byte R, byte G, byte B)
{
    public static readonly HexColor White = new(255, 255, 255);
    public static readonly HexColor Black = new(0, 0, 0);

    /// <summary>Luminancia relativa según WCAG 2.x (0 = negro, 1 = blanco).</summary>
    public double RelativeLuminance =>
        0.2126 * Linearize(R) + 0.7152 * Linearize(G) + 0.0722 * Linearize(B);

    public static HexColor Parse(string value) =>
        TryParse(value, out var color)
            ? color
            : throw new FormatException($"Color hexadecimal inválido: '{value}'. Se espera el formato #RRGGBB.");

    public static bool TryParse(string? value, out HexColor color)
    {
        color = default;

        if (value is not { Length: 7 } || value[0] != '#')
        {
            return false;
        }

        var span = value.AsSpan();
        if (byte.TryParse(span.Slice(1, 2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out var r)
            && byte.TryParse(span.Slice(3, 2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out var g)
            && byte.TryParse(span.Slice(5, 2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out var b))
        {
            color = new HexColor(r, g, b);
            return true;
        }

        return false;
    }

    /// <summary>Relación de contraste WCAG entre dos colores (1 a 21).</summary>
    public static double ContrastRatio(HexColor a, HexColor b)
    {
        var la = a.RelativeLuminance;
        var lb = b.RelativeLuminance;
        var lighter = Math.Max(la, lb);
        var darker = Math.Min(la, lb);
        return (lighter + 0.05) / (darker + 0.05);
    }

    /// <summary>Blanco o negro, el que tenga mejor contraste sobre este color.</summary>
    public HexColor ContrastingText() =>
        ContrastRatio(this, White) >= ContrastRatio(this, Black) ? White : Black;

    /// <summary>Interpolación lineal hacia <paramref name="other"/> (0 = este color, 1 = el otro).</summary>
    public HexColor Mix(HexColor other, double amount)
    {
        var t = Math.Clamp(amount, 0.0, 1.0);
        return new HexColor(Lerp(R, other.R, t), Lerp(G, other.G, t), Lerp(B, other.B, t));
    }

    public override string ToString() => $"#{R:x2}{G:x2}{B:x2}";

    private static byte Lerp(byte from, byte to, double t) =>
        (byte)Math.Round(from + (to - from) * t);

    private static double Linearize(byte channel)
    {
        var s = channel / 255.0;
        return s <= 0.03928 ? s / 12.92 : Math.Pow((s + 0.055) / 1.055, 2.4);
    }
}
