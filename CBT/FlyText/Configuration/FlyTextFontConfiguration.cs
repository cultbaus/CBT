namespace CBT.FlyText.Configuration;

using System.Numerics;

/// <summary>
/// FlyTextFont configuration options.
/// </summary>
internal class FlyTextFontConfiguration
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FlyTextFontConfiguration"/> class.
    /// </summary>
    /// <param name="size">Size of the font in pixels.</param>
    /// <param name="name">Name of the font.</param>
    /// <param name="color">Color of the font.</param>
    /// <param name="format">Whether or not to format the value of the event.</param>
    internal FlyTextFontConfiguration(float size, string name, Vector4 color, bool format)
    {
        this.Size = size;
        this.Name = name;
        this.Color = color;
        this.Format = format;
    }

    /// <summary>
    ///  Gets or sets the size of the font.
    /// </summary>
    internal float Size { get; set; }

    /// <summary>
    /// Gets or sets the name of the font.
    /// </summary>
    internal string Name { get; set; }

    /// <summary>
    /// Gets or sets the color of the font.
    /// </summary>
    internal Vector4 Color { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether or not the text should be formatted.
    /// </summary>
    internal bool Format { get; set; }
}