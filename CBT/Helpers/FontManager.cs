namespace CBT.Helpers;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Dalamud.Interface.ManagedFontAtlas;

/// <summary>
/// FontManager loads fonts from the Media directory and exposes a Push API to scope font handles.
/// </summary>
public class FontManager : IDisposable
{
    private readonly string mediaPath;
    private readonly List<Font> fonts = new List<Font>();

    /// <summary>
    /// Initializes a new instance of the <see cref="FontManager"/> class.
    /// </summary>
    /// <param name="mediaPath">Path to the Media directory where fonts are located.</param>
    public FontManager(string mediaPath)
    {
        this.mediaPath = mediaPath;

        this.LoadAllFonts();
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        this.fonts?.Clear();
    }

    /// <summary>
    /// Pushes a Dalamud FontHandle into the current scope.
    /// </summary>
    /// <param name="name">Name of the font to push.</param>
    /// <param name="size">Size of the font to push.</param>
    /// <returns>A <see cref="Font"/> instance which will Pop once it goes out of scope.</returns>
    public Font? Push(string name, float size)
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

        PluginConfiguration.Fonts = this.fonts
            .GroupBy(font => font.Name)
            .ToDictionary(
                font =>
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
            {
                this.fonts.Add(new Font(fontHandle, fontName, size));
            }
        }
        catch (Exception ex)
        {
            Service.PluginLog.Error($"Error loading font from media path: {ex.Message}");
        }
    }
}