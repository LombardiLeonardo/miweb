using System.Text;
using Catalogo.Domain;

namespace Catalogo.Rendering;

public sealed class FooterBlockRenderer : BlockRenderer<FooterBlock>
{
    protected override string RenderBlock(FooterBlock block, RenderContext context)
    {
        var links = block.Links ?? [];

        var html = new StringBuilder();
        html.AppendLine("<footer class=\"site-footer\">");
        html.AppendLine("  <div class=\"container\">");

        if (links.Count > 0)
        {
            html.AppendLine("    <ul>");
            foreach (var link in links)
            {
                html.AppendLine($"      <li><a href=\"{Html.SafeUrl(link.Url)}\">{Html.Encode(link.Label)}</a></li>");
            }

            html.AppendLine("    </ul>");
        }

        html.AppendLine($"    <p>{Html.Encode(block.Text)}</p>");
        html.AppendLine("  </div>");
        html.Append("</footer>");
        return html.ToString();
    }
}
