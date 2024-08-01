namespace CBT.Helpers;

using System;

using Dalamud.Interface.ManagedFontAtlas;

/// <summary>
/// Font is a Dalamud IFontHandle with a specific name and size.
/// </summary>
public class Font : IDisposable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Font"/> class.
    /// </summary>
    /// <param name="handle">Dalamud font handle.</param>
    /// <param name="name">Font name.</param>
    /// <param name="size">Font size in pixels.</param>
    public Font(IFontHandle handle, string name, float size)
    {
        this.Handle = handle;
        this.Name = name;
        this.Size = size;
    }

    /// <summary>
    /// Gets or sets the Dalamud IFontHandle.
    /// </summary>
    public IFontHandle Handle { get; set; }

    /// <summary>
    /// Gets or sets the name of the font.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the size of the font in pixels.
    /// </summary>
    public float Size { get; set; }

    /// <inheritdoc/>
    public void Dispose()
        => this.Handle?.Pop();

    /// <summary>
    /// Pushes the Dalamud IFontHandle into the ImGUI stack.
    /// </summary>
    /// <returns>A Dalamud IFontHandle scope.</returns>
    public Font Push()
    {
        this.Handle?.Push();
        return this;
    }
}