namespace CBT.FlyText.Configuration;

using System.Numerics;

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
        this.Size = Defaults.DefaultFontSize;
        this.Name = Defaults.DefaultFontName;
        this.Color = Defaults.DefaultFontColor;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FlyTextFontConfiguration"/> class.
    /// </summary>
    /// <param name="size">Size of the font in pixels.</param>
    /// <param name="name">Name of the font.</param>
    /// <param name="color">Color of the font.</param>
    /// <param name="outlineConfig">Text outline configuration.</param>
    public FlyTextFontConfiguration(float size, string name, Vector4 color, FlyTextOutlineConfiguration outlineConfig)
    {
        this.Outline = outlineConfig;
        this.Size = size;
        this.Name = name;
        this.Color = color;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FlyTextFontConfiguration"/> class.
    /// </summary>
    /// <param name="toCopy">Copies values from this.</param>
    public FlyTextFontConfiguration(FlyTextFontConfiguration toCopy)
    {
        this.Outline = toCopy.Outline;
        this.Size = toCopy.Size;
        this.Name = toCopy.Name;
        this.Color = toCopy.Color;
    }

    /// <summary>
    /// Gets or sets the outline configuration.
    /// </summary>
    public FlyTextOutlineConfiguration Outline { get; set; }

    /// <summary>
    ///  Gets or sets the size of the font.
    /// </summary>
    public float Size { get; set; }

    /// <summary>
    /// Gets or sets the name of the font.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the color of the font.
    /// </summary>
    public Vector4 Color { get; set; }
}