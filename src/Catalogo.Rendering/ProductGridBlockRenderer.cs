using System.Globalization;
using System.Text;
using Catalogo.Domain;

namespace Catalogo.Rendering;

public sealed class ProductGridBlockRenderer : BlockRenderer<ProductGridBlock>
{
    protected override string RenderBlock(ProductGridBlock block, RenderContext context)
    {
        var layout = block.Layout switch
        {
            GridLayout.Grid => "grid",
            GridLayout.List => "list",
            GridLayout.Cards => "cards",
            _ => throw new ArgumentOutOfRangeException(nameof(block), block.Layout, "Layout no soportado."),
        };
        var columns = Math.Clamp(block.Columns, 1, 6);

        var html = new StringBuilder();
        html.AppendLine("<section class=\"section\">");
        html.AppendLine("  <div class=\"container\">");
        html.AppendLine($"    <h2>{Html.Encode(block.Heading)}</h2>");
        html.AppendLine($"    <div class=\"product-grid product-grid--{layout}\" style=\"--columns: {columns}\">");

        var culture = CultureInfo.GetCultureInfo(context.Site.Locale);

        foreach (var product in SelectProducts(block, context.Site.Products))
        {
            AppendProduct(html, product, culture);
        }

        html.AppendLine("    </div>");
        html.AppendLine("  </div>");
        html.Append("</section>");
        return html.ToString();
    }

    private static IEnumerable<Product> SelectProducts(ProductGridBlock block, IReadOnlyList<Product> all)
    {
        if (block.ProductIds is null)
        {
            return all;
        }

        var byId = all.ToDictionary(p => p.Id);
        return block.ProductIds
            .Select(id => byId.GetValueOrDefault(id))
            .OfType<Product>();
    }

    private static void AppendProduct(StringBuilder html, Product product, CultureInfo culture)
    {
        var price = $"{product.Currency} {product.Price.ToString("N2", culture)}";

        html.AppendLine("      <article class=\"product-card\">");

        if (!string.IsNullOrWhiteSpace(product.ImageUrl))
        {
            html.AppendLine($"        <img src=\"{Html.SafeUrl(product.ImageUrl)}\" alt=\"{Html.Encode(product.Name)}\" loading=\"lazy\">");
        }

        html.AppendLine("        <div class=\"product-card__body\">");
        html.AppendLine($"          <h3>{Html.Encode(product.Name)}</h3>");

        if (!string.IsNullOrWhiteSpace(product.Description))
        {
            html.AppendLine($"          <p>{Html.Encode(product.Description)}</p>");
        }

        html.AppendLine($"          <p class=\"product-card__price\">{Html.Encode(price)}</p>");
        html.AppendLine("        </div>");
        html.AppendLine("      </article>");
    }
}
