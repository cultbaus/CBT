namespace Scroll.Helpers;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Dalamud.Interface.ManagedFontAtlas;

internal partial class FontManager
{
    private readonly string mediaPath;
    private readonly List<Font> fonts = new List<Font>();

    internal FontManager(string mediaPath)
    {
        this.mediaPath = mediaPath;

        this.LoadAllFonts();
    }

    public void Dispose()
    {
        this.fonts?.Clear();
    }

    internal Font? Push(string name, float size)
    {
        return this.fonts.FirstOrDefault(f => f.Name == name && f.Size == size)?.Push();
    }

    private void LoadAllFonts()
    {
        Directory.GetFiles(this.mediaPath, "*.ttf")
            .Select(file => Path.GetFileNameWithoutExtension(file))
            .ToList()
            .ForEach(file =>
            {
                Enumerable.Range(14, 32 - 14 + 1)
                    .Where(i => i % 2 == 0)
                    .ToList()
                    .ForEach(size =>
                    {
                        this.LoadFont(file, size);
                    });

            });

        Service.Configuration.Fonts = this.fonts
            .GroupBy(font => font.Name)
            .ToDictionary(font =>
                font.Key,
                font => font.Select(f => f.Size).ToList());
    }

    private void LoadFont(string fontName, float size)
    {
        try
        {
            IFontHandle fontHandle = Service.Interface.UiBuilder.FontAtlas.NewDelegateFontHandle(e =>
            {
                e.OnPreBuild(tk => tk.AddFontFromFile(Path.Combine(this.mediaPath, $"{fontName}.ttf"), new SafeFontConfig { SizePx = size }));
            });

            if (fontHandle != null)
                this.fonts.Add(new Font(fontHandle, fontName, size));
        }
        catch (Exception ex)
        {
            Service.PluginLog.Error($"Error loading font from media path: {ex.Message}");
        }
    }
}