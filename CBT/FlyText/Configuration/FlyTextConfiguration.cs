namespace CBT.FlyText.Configuration;

using CBT.FlyText.Types;

/// <summary>
/// FlyText configuration options.
/// </summary>
public class FlyTextConfiguration
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FlyTextConfiguration"/> class.
    /// </summary>
    public FlyTextConfiguration()
    {
        this.Font = new FlyTextFontConfiguration(Defaults.DefaultFontSize, Defaults.DefaultFontName, Defaults.DefaultFontColor);
        this.Outline = new FlyTextOutlineConfiguration(Defaults.DefaultOutlineEnabled, Defaults.DefaultOutlineThickness, Defaults.DefaultOutlineColor);
        this.Animation = new FlyTextAnimationConfiguration(Defaults.DefaultAnimationKind, Defaults.DefaultAnimationDuration, Defaults.DefaultAnimationSpeed);
        this.Icon = new FlyTextIconConfiguration(Defaults.DefaultIconSize, Defaults.DefaultIconZoom, Defaults.DefaultIconEnabled, new FlyTextOutlineConfiguration(this.Outline));
        this.Message = new FlyTextMessageConfiguration(string.Empty, string.Empty, Defaults.DefaultMessageFormat);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FlyTextConfiguration"/> class.
    /// </summary>
    /// <param name="fontConfig">Font config.</param>
    /// <param name="outlineConfig">Outline config.</param>
    /// <param name="animationConfig">Animation config.</param>
    /// <param name="iconConfig">Icon config.</param>
    /// <param name="messageConfig">Message config.</param>
    public FlyTextConfiguration(FlyTextFontConfiguration fontConfig, FlyTextOutlineConfiguration outlineConfig, FlyTextAnimationConfiguration animationConfig, FlyTextIconConfiguration iconConfig, FlyTextMessageConfiguration messageConfig)
    {
        this.Font = fontConfig;
        this.Outline = outlineConfig;
        this.Animation = animationConfig;
        this.Icon = iconConfig;
        this.Message = messageConfig;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FlyTextConfiguration"/> class.
    /// </summary>
    /// <param name="kind">FlyTextKind to read configuration for.</param>
    public FlyTextConfiguration(FlyTextKind kind)
    {
        FlyTextConfiguration config = Service.Configuration.FlyTextKinds[kind];

        this.Font = new FlyTextFontConfiguration(config.Font.Size, config.Font.Name, config.Font.Color);
        this.Outline = new FlyTextOutlineConfiguration(config.Outline.Enabled, config.Outline.Size, config.Outline.Color);
        this.Animation = new FlyTextAnimationConfiguration(config.Animation.Kind, config.Animation.Duration, config.Animation.Speed);
        this.Icon = new FlyTextIconConfiguration(config.Icon.Size, config.Icon.Zoom, config.Icon.Enabled, config.Icon.OutlineConfig);
        this.Message = new FlyTextMessageConfiguration(config.Message.Prefix, config.Message.Suffix, config.Message.Format);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FlyTextConfiguration"/> class.
    /// </summary>
    /// <param name="toCopy">Values to copy.</param>
    public FlyTextConfiguration(FlyTextConfiguration toCopy)
    {
        this.Font = new FlyTextFontConfiguration(toCopy.Font);
        this.Outline = new FlyTextOutlineConfiguration(toCopy.Outline);
        this.Animation = new FlyTextAnimationConfiguration(toCopy.Animation);
        this.Icon = new FlyTextIconConfiguration(toCopy.Icon);
        this.Message = new FlyTextMessageConfiguration(toCopy.Message);
    }

    /// <summary>
    /// Gets or sets a value indicating whether or not the kind is enabled.
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Gets or sets the font configuration.
    /// </summary>
    public FlyTextFontConfiguration Font { get; set; }

    /// <summary>
    /// Gets or sets the outline configuration.
    /// </summary>
    public FlyTextOutlineConfiguration Outline { get; set; }

    /// <summary>
    /// Gets or sets the animation configuration.
    /// </summary>
    public FlyTextAnimationConfiguration Animation { get; set; }

    /// <summary>
    /// Gets or sets the icon configuration.
    /// </summary>
    public FlyTextIconConfiguration Icon { get; set; }

    /// <summary>
    /// Gets or sets the message configuration.
    /// </summary>
    public FlyTextMessageConfiguration Message { get; set; }
}