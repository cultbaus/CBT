namespace CBT.FlyText.Configuration;

using System.Numerics;

/// <summary>
/// FlyTextOutline configuration options.
/// </summary>
public class FlyTextOutlineConfiguration
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FlyTextOutlineConfiguration"/> class.
    /// </summary>
    /// <param name="enabled">If the outline is enabled.</param>
    /// <param name="size">Size of the outline in pixels.</param>
    /// <param name="color">Color of the outline.</param>
    public FlyTextOutlineConfiguration(bool enabled, int size, Vector4 color)
    {
        this.Enabled = enabled;
        this.Size = size;
        this.Color = color;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the outline is enabled.
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// Gets or sets the Size of the outline.
    /// </summary>
    public int Size { get; set; }

    /// <summary>
    /// Gets or sets the color of the outline.
    /// </summary>
    public Vector4 Color { get; set; }
}