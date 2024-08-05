namespace CBT.FlyText.Configuration;

using System.Numerics;

/// <summary>
/// FlyText Icon configuration.
/// </summary>
public class FlyTextIconConfiguration
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FlyTextIconConfiguration"/> class.
    /// </summary>
    /// <param name="iconSize">Size of the icon in pixels.</param>
    /// <param name="iconZoom">Zoom ratio of the icon.</param>
    /// <param name="iconEnabled">Icon is enabled.</param>
    /// <param name="outlineConfig">Outline configuration.</param>
    public FlyTextIconConfiguration(Vector2 iconSize, float iconZoom, bool iconEnabled, FlyTextOutlineConfiguration outlineConfig)
    {
        this.OutlineConfig = outlineConfig;
        this.Size = iconSize;
        this.Zoom = iconZoom;
        this.Enabled = iconEnabled;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FlyTextIconConfiguration"/> class.
    /// </summary>
    /// <param name="toCopy">Configuration to copy values from.</param>
    public FlyTextIconConfiguration(FlyTextIconConfiguration toCopy)
    {
        this.OutlineConfig = new FlyTextOutlineConfiguration(toCopy.OutlineConfig);
        this.Size = toCopy.Size;
        this.Zoom = toCopy.Zoom;
        this.Enabled = toCopy.Enabled;
    }

    /// <summary>
    /// Gets or sets the outline configuration.
    /// </summary>
    public FlyTextOutlineConfiguration OutlineConfig { get; set; }

    /// <summary>
    /// Gets or sets the size of the icon.
    /// </summary>
    public Vector2 Size { get; set; }

    /// <summary>
    /// Gets or sets the zoom ratio of the icon.
    /// </summary>
    public float Zoom { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether an icon is enabled.
    /// </summary>
    public bool Enabled { get; set; }
}