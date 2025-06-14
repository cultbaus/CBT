namespace CBT.FlyText.Configuration;

using System.Numerics;
using Dalamud.Interface.FontIdentifier;

/// <summary>
/// FlyTextFont configuration options.
/// </summary>
public class FlyTextFontConfiguration
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FlyTextFontConfiguration"/> class.
    /// </summary>
    public FlyTextFontConfiguration()
    {
        this.Outline = new FlyTextOutlineConfiguration();
        this.FontId = Defaults.DefaultFontId;
        this.Size = Defaults.DefaultFontSize;
        this.Name = this.FontId.EnglishName;
        this.Color = Defaults.DefaultFontColor;
        this.ColorSuccess = Defaults.DefaultFontColor;
        this.ColorFailed = Defaults.DefaultFontColor;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FlyTextFontConfiguration"/> class.
    /// </summary>
    /// <param name="toCopy">Copies values from this.</param>
    public FlyTextFontConfiguration(FlyTextFontConfiguration toCopy)
    {
        this.Outline = toCopy.Outline;
        this.FontId = toCopy.FontId;
        this.Size = toCopy.Size;
        this.Name = toCopy.Name;
        this.Color = toCopy.Color;
        this.ColorSuccess = toCopy.ColorSuccess;
        this.ColorFailed = toCopy.ColorFailed;
    }

    /// <summary>
    /// Gets or sets the outline configuration.
    /// </summary>
    public FlyTextOutlineConfiguration Outline { get; set; }

    /// <summary>
    ///  Gets or sets the font id.
    /// </summary>
    public IFontId FontId { get; set; }

    /// <summary>
    ///  Gets or sets the size of the font.
    /// </summary>
    public float Size { get; set; }

    /// <summary>
    /// Gets the name of the font.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets or sets the color of the font.
    /// </summary>
    public Vector4 Color { get; set; }

    /// <summary>
    /// Gets or sets the color of the font for positional success.
    /// </summary>
    public Vector4 ColorSuccess { get; set; }

    /// <summary>
    /// Gets or sets the color of the font for positional failures.
    /// </summary>
    public Vector4 ColorFailed { get; set; }
}