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
    public FlyTextIconConfiguration()
    {
        this.Size = Defaults.DefaultIconSize;
        this.Zoom = Defaults.DefaultIconZoom;
        this.Offset = Defaults.DefaultIconOffset;
        this.Enabled = Defaults.DefaultIconEnabled;
        this.Outline = new FlyTextOutlineConfiguration();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FlyTextIconConfiguration"/> class.
    /// </summary>
    /// <param name="iconSize">Size of the icon in pixels.</param>
    /// <param name="iconZoom">Zoom ratio of the icon.</param>
    /// <param name="iconEnabled">Icon is enabled.</param>
    /// <param name="iconOffset">Icon offset from the text.</param>
    /// <param name="outlineConfig">Outline configuration.</param>
    public FlyTextIconConfiguration(Vector2 iconSize, float iconZoom, Vector2 iconOffset, bool iconEnabled, FlyTextOutlineConfiguration outlineConfig)
    {
        this.Outline = outlineConfig;
        this.Size = iconSize;
        this.Offset = iconOffset;
        this.Zoom = iconZoom;
        this.Enabled = iconEnabled;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FlyTextIconConfiguration"/> class.
    /// </summary>
    /// <param name="toCopy">Configuration to copy values from.</param>
    public FlyTextIconConfiguration(FlyTextIconConfiguration toCopy)
    {
        this.Outline = new FlyTextOutlineConfiguration(toCopy.Outline);
        this.Size = toCopy.Size;
        this.Offset = toCopy.Offset;
        this.Zoom = toCopy.Zoom;
        this.Enabled = toCopy.Enabled;
    }

    /// <summary>
    /// Gets or sets the outline configuration.
    /// </summary>
    public FlyTextOutlineConfiguration Outline { get; set; }

    /// <summary>
    /// Gets or sets the size of the icon.
    /// </summary>
    public Vector2 Size { get; set; }

    /// <summary>
    /// Gets or sets the offset of the icon.
    /// </summary>
    public Vector2 Offset { get; set; }

    /// <summary>
    /// Gets or sets the zoom ratio of the icon.
    /// </summary>
    public float Zoom { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether an icon is enabled.
    /// </summary>
    public bool Enabled { get; set; }
}