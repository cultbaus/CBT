using System.Collections.Concurrent;

namespace CBT.Helpers;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CBT.FlyText.Configuration;
using Dalamud.Interface.FontIdentifier;
using Dalamud.Interface.GameFonts;
using Dalamud.Interface.ManagedFontAtlas;

/// <summary>
/// FontManager loads fonts from the Media directory and exposes a Push API to scope font handles.
/// </summary>
public class FontManager : IDisposable
{
    private readonly ConcurrentDictionary<(IFontId, float), IFontHandle> fonts = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="FontManager"/> class.
    /// </summary>
    public FontManager()
    {
        Enumerable.Range(14, 32 - 14 + 1)
            .Where(i => i % 2 == 0)
            .ToList()
            .ForEach(size =>
            {
                this.BuildFont(Defaults.DefaultFontId, size);
            });

        foreach (var k in Service.Configuration.FlyTextKinds)
        {
            this.BuildFont(k.Value.Font.FontId, k.Value.Font.Size);
        }
    }

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
    public IDisposable? Push(IFontId fontId, float size)
    {
        if (this.fonts.TryGetValue((fontId, size), out var fontHandle))
        {
            return fontHandle.Push();
        }

        Service.PluginLog.Error($"CBT FontManager failed to push font {fontId} with size {size}.");
        return null;
    }

    /// <summary>
    /// Builds a FontHandle and adds it to the FontManager.
    /// </summary>
    /// <param name="fontId">ID of the font to add.</param>
    /// <param name="size">Size of the font to add.</param>
    public void BuildFont(IFontId fontId, float size)
    {
        if (this.fonts.ContainsKey((fontId, size)))
        {
            return;
        }

        IFontHandle fontHandle = Service.Interface.UiBuilder.FontAtlas.NewDelegateFontHandle(e => e.OnPreBuild(tk =>
        {
            var cfg = new SafeFontConfig { SizePt = size };
            cfg.MergeFont = fontId.AddToBuildToolkit(tk, cfg);

            tk.Font = cfg.MergeFont;
        }));
        this.fonts.TryAdd((fontId, size), fontHandle);
    }
}