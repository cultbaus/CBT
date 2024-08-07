namespace CBT.FlyText.Configuration;

using CBT.Types;

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
        this.Font = new FlyTextFontConfiguration();
        this.Animation = new FlyTextAnimationConfiguration();
        this.Icon = new FlyTextIconConfiguration();
        this.Message = new FlyTextMessageConfiguration();
        this.Filter = new FlyTextFilterConfiguration();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FlyTextConfiguration"/> class.
    /// </summary>
    /// <param name="kind">FlyTextKind to read configuration for.</param>
    public FlyTextConfiguration(FlyTextKind kind)
    {
        FlyTextConfiguration config = Service.Configuration.FlyTextKinds[kind];

        this.Font = new FlyTextFontConfiguration(config.Font);
        this.Animation = new FlyTextAnimationConfiguration(config.Animation);
        this.Icon = new FlyTextIconConfiguration(config.Icon);
        this.Message = new FlyTextMessageConfiguration(config.Message);
        this.Filter = new FlyTextFilterConfiguration(config.Filter);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FlyTextConfiguration"/> class.
    /// </summary>
    /// <param name="toCopy">Values to copy.</param>
    public FlyTextConfiguration(FlyTextConfiguration toCopy)
    {
        this.Enabled = toCopy.Enabled;
        this.Font = new FlyTextFontConfiguration(toCopy.Font);
        this.Animation = new FlyTextAnimationConfiguration(toCopy.Animation);
        this.Icon = new FlyTextIconConfiguration(toCopy.Icon);
        this.Message = new FlyTextMessageConfiguration(toCopy.Message);
        this.Filter = new FlyTextFilterConfiguration(toCopy.Filter);
    }

    /// <summary>
    /// Gets or sets a value indicating whether or not the kind is enabled.
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// Gets or sets the font configuration.
    /// </summary>
    public FlyTextFontConfiguration Font { get; set; }

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

    /// <summary>
    /// Gets or sets the filter configuration.
    /// </summary>
    public FlyTextFilterConfiguration Filter { get; set; }
}