using System.Text;
using Catalogo.Domain;

namespace Catalogo.Rendering;

public sealed class HeroBlockRenderer : BlockRenderer<HeroBlock>
{
    protected override string RenderBlock(HeroBlock block, RenderContext context)
    {
        var hasImage = !string.IsNullOrWhiteSpace(block.ImageUrl);
        var modifier = hasImage ? " hero__inner--with-image" : string.Empty;

        var html = new StringBuilder();
        html.AppendLine("<section class=\"hero\">");
        html.AppendLine($"  <div class=\"container hero__inner{modifier}\">");
        html.AppendLine("    <div class=\"hero__content\">");
        html.AppendLine($"      <h1>{Html.Encode(block.Title)}</h1>");

        if (!string.IsNullOrWhiteSpace(block.Subtitle))
        {
            html.AppendLine($"      <p>{Html.Encode(block.Subtitle)}</p>");
        }

        if (!string.IsNullOrWhiteSpace(block.CtaText))
        {
            html.AppendLine($"      <a class=\"btn\" href=\"{Html.SafeUrl(block.CtaUrl)}\">{Html.Encode(block.CtaText)}</a>");
        }

        html.AppendLine("    </div>");

        if (hasImage)
        {
            html.AppendLine($"    <img src=\"{Html.SafeUrl(block.ImageUrl)}\" alt=\"{Html.Encode(block.Title)}\">");
        }

        html.AppendLine("  </div>");
        html.Append("</section>");
        return html.ToString();
    }
}
