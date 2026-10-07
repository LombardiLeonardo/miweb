namespace Catalogo.Rendering;

/// <summary>CSS base de los bloques. Solo usa variables del tema, nunca colores fijos.</summary>
internal static class BaseStyles
{
    public const string Css = """
        *, *::before, *::after { box-sizing: border-box; }
        body { margin: 0; font-family: var(--font-family); background: var(--color-bg); color: var(--color-text); line-height: 1.5; }
        img { max-width: 100%; display: block; }
        a { color: var(--color-primary); }
        .container { width: min(1100px, 100% - 2rem); margin-inline: auto; }
        .section { padding: 3rem 0; }
        .section h2 { margin: 0 0 1.5rem; }

        .btn { display: inline-block; padding: .75rem 1.5rem; border-radius: var(--radius); background: var(--color-primary); color: var(--color-on-primary); text-decoration: none; font-weight: 600; }
        .btn:hover { background: var(--color-primary-hover); }

        .hero { padding: 5rem 0; background: var(--color-surface); }
        .hero__inner { display: grid; gap: 2rem; align-items: center; }
        @media (min-width: 800px) { .hero__inner--with-image { grid-template-columns: 1fr 1fr; } }
        .hero h1 { font-size: clamp(2rem, 5vw, 3.5rem); margin: 0 0 1rem; line-height: 1.1; }
        .hero p { font-size: 1.15rem; color: var(--color-muted); margin: 0 0 1.5rem; }
        .hero img { border-radius: var(--radius); }

        .product-grid { display: grid; gap: 1.25rem; }
        .product-grid--grid { grid-template-columns: repeat(var(--columns, 3), minmax(0, 1fr)); }
        @media (max-width: 700px) { .product-grid--grid { grid-template-columns: repeat(2, minmax(0, 1fr)); } }
        @media (max-width: 420px) { .product-grid--grid { grid-template-columns: 1fr; } }
        .product-grid--list { grid-template-columns: 1fr; }
        .product-grid--list .product-card { display: flex; align-items: center; }
        .product-grid--list .product-card img { width: 140px; flex: none; }
        .product-grid--list .product-card__body { flex: 1; }
        .product-grid--cards { grid-template-columns: repeat(auto-fill, minmax(min(100%, 320px), 1fr)); }
        .product-grid--cards .product-card { box-shadow: 0 4px 16px rgb(0 0 0 / .08); }

        .product-card { background: var(--color-surface); border: 1px solid var(--color-border); border-radius: var(--radius); overflow: hidden; }
        .product-card__body { padding: 1rem; }
        .product-card h3 { margin: 0 0 .25rem; font-size: 1.05rem; }
        .product-card p { margin: 0 0 .5rem; color: var(--color-muted); }
        .product-card .product-card__price { font-weight: 700; color: var(--color-text); }

        .site-footer { padding: 2rem 0; border-top: 1px solid var(--color-border); color: var(--color-muted); }
        .site-footer ul { list-style: none; display: flex; flex-wrap: wrap; gap: 1rem; padding: 0; margin: 0 0 1rem; }
        .site-footer p { margin: 0; }
        """;
}
