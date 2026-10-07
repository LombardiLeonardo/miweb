namespace Catalogo.Domain;

/// <summary>
/// Design tokens elegidos por el cliente. Solo se guardan los valores "de entrada";
/// el resto (hover, bordes, texto sobre primario...) se deriva en el renderer.
/// </summary>
public sealed record Theme(
    string Name,
    string Primary,
    string Background = "#ffffff",
    string Text = "#1f2933",
    string FontFamily = "system-ui, -apple-system, 'Segoe UI', sans-serif",
    int RadiusPx = 8)
{
    public static Theme Default { get; } = new("default", "#2563eb");
}
