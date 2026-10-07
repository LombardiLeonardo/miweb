using Catalogo.Domain;

namespace Catalogo.Rendering;

/// <summary>Contexto disponible para cualquier bloque al renderizarse.</summary>
public sealed record RenderContext(Site Site, Page Page);

/// <summary>Convierte un bloque del modelo en un fragmento HTML.</summary>
public interface IBlockRenderer
{
    bool CanRender(Block block);

    string Render(Block block, RenderContext context);
}

/// <summary>
/// Base tipada: cada renderer concreto declara el bloque que maneja y recibe el tipo ya convertido.
/// </summary>
public abstract class BlockRenderer<TBlock> : IBlockRenderer
    where TBlock : Block
{
    public bool CanRender(Block block) => block is TBlock;

    public string Render(Block block, RenderContext context) =>
        RenderBlock((TBlock)block, context);

    protected abstract string RenderBlock(TBlock block, RenderContext context);
}
