using Microsoft.Extensions.DependencyInjection;

namespace Catalogo.Rendering;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registra los renderers incluidos y el <see cref="PageRenderer"/>.
    /// Un bloque nuevo solo necesita su propio <c>AddSingleton&lt;IBlockRenderer, ...&gt;</c>.
    /// </summary>
    public static IServiceCollection AddCatalogoRendering(this IServiceCollection services)
    {
        services.AddSingleton<IBlockRenderer, HeroBlockRenderer>();
        services.AddSingleton<IBlockRenderer, ProductGridBlockRenderer>();
        services.AddSingleton<IBlockRenderer, FooterBlockRenderer>();
        services.AddSingleton<PageRenderer>();
        return services;
    }
}
