namespace CBT.Helpers;

using System;
using System.Collections.Concurrent;
using Dalamud.Interface.FontIdentifier;
using Dalamud.Interface.ManagedFontAtlas;

/// <summary>
/// FontManager loads fonts from the Media directory and exposes a Push API to scope font handles.
/// </summary>
public class FontManager : IDisposable
{
    private readonly ConcurrentDictionary<(IFontId, float), Lazy<IFontHandle>> fonts = [];

    /// <inheritdoc/>
    public void Dispose()
    {
        this.fonts?.Clear();
    }

    /// <summary>
    /// Pushes a Dalamud FontHandle into the current scope.
    /// </summary>
    /// <param name="fontId">ID of the font to push.</param>
    /// <param name="size">Size of the font to push.</param>
    /// <returns>A <see cref="IDisposable"/> font object which will Pop once it goes out of scope.</returns>
    public IDisposable Push(IFontId fontId, float size)
    {
        return this.fonts.GetOrAdd((fontId, size), key => new Lazy<IFontHandle>(() => BuildFontHandle(key.Item1, key.Item2))).Value.Push();
    }

    /// <summary>
    /// Builds and returns a FontHandle.
    /// </summary>
    /// <param name="fontId">ID of the font to add.</param>
    /// <param name="size">Size of the font to add.</param>
    private static IFontHandle BuildFontHandle(IFontId fontId, float size)
    {
        return Service.Interface.UiBuilder.FontAtlas.NewDelegateFontHandle(e => e.OnPreBuild(tk =>
        {
            var cfg = new SafeFontConfig { SizePt = size };
            cfg.MergeFont = fontId.AddToBuildToolkit(tk, cfg);

            tk.Font = cfg.MergeFont;
        }));
    }
}