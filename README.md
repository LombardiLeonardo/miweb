# Catalogo

Generador de sitios/catálogos en C#: contenido + tema + bloques → HTML estático.

## Estructura

```
src/Catalogo.Domain      Modelo: Site, Page, Block (hero, product-grid, footer), Theme, Product, HexColor
src/Catalogo.Rendering   IBlockRenderer, renderers por bloque, ThemeCssBuilder, PageRenderer
src/Catalogo.Cli         Genera un sitio de ejemplo con 3 paletas en ./out
tests/Catalogo.Tests     xUnit + FluentAssertions
docs/adr                 Decisiones de arquitectura
```

## Comandos

```
dotnet test
dotnet run --project src/Catalogo.Cli
```

Luego abrir `out/azul/index.html`, `out/verde/index.html` y `out/noche/index.html` en el navegador.

Requiere el SDK de .NET 10. Si usas otro, cambia `TargetFramework` en `Directory.Build.props`
(y las versiones 10.0.0 de los paquetes `Microsoft.Extensions.*`).

## Agregar un bloque nuevo

1. Crear el record en `Blocks.cs` y registrarlo con `[JsonDerivedType(typeof(X), "x")]`.
2. Crear `XBlockRenderer : BlockRenderer<XBlock>`.
3. Registrarlo en `AddCatalogoRendering` (y en `PageRenderer.CreateDefault`).
4. Escribir sus pruebas.
