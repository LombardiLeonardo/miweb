# ADR 0001: Monolito modular y un renderer por tipo de bloque

- Estado: aceptada
- Fecha: 2026-10-07

## Contexto

El producto genera sitios (estáticos o tiendas) a partir de contenido, un tema y una lista de bloques.
Hay un solo desarrollador y el objetivo incluye aprender C# a fondo.

## Decisión

- Una sola solución con proyectos separados por responsabilidad (Domain, Rendering, más adelante Application/Infrastructure/Web), sin microservicios.
- Domain no depende de nada. Rendering solo depende de Domain.
- Cada tipo de bloque tiene su propio `BlockRenderer<T>` registrado en DI. Agregar un bloque no modifica `PageRenderer`.
- El tema se guarda como pocos valores de entrada (colores, tipografía, radio); el resto se deriva (contraste WCAG, hover, bordes).
- Todo dato del cliente se escapa al renderizar y las URLs se validan (`Html.SafeUrl`).

## Consecuencias

- Agregar un bloque exige tocar tres sitios: record en Domain + `[JsonDerivedType]`, su renderer y su registro en DI.
- Los renderers producen HTML como texto; si crece la complejidad, evaluar Razor/Scriban sin cambiar `IBlockRenderer`.
- Pendiente: snapshot testing (Verify) del HTML generado.
