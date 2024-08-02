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
    /// <param name="size">Size of the font in pixels.</param>
    /// <param name="name">Name of the font.</param>
    /// <param name="color">Color of the font.</param>
    /// <param name="format">Whether or not to format the value of the event.</param>
    public FlyTextFontConfiguration(float size, string name, Vector4 color, bool format)
    {
        this.Size = size;
        this.Name = name;
        this.Color = color;
        this.Format = format;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FlyTextFontConfiguration"/> class.
    /// </summary>
    /// <param name="toCopy">Copies values from this.</param>
    public FlyTextFontConfiguration(FlyTextFontConfiguration toCopy)
    {
        this.Size = toCopy.Size;
        this.Name = toCopy.Name;
        this.Color = toCopy.Color;
        this.Format = toCopy.Format;
    }

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

    /// <summary>
    /// Gets or sets a value indicating whether or not the text should be formatted.
    /// </summary>
    public bool Format { get; set; }
}