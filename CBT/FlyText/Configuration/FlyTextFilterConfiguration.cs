namespace CBT.FlyText.Configuration;

/// <summary>
/// FlyTextFilter configuration.
/// </summary>
public class FlyTextFilterConfiguration
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FlyTextFilterConfiguration"/> class.
    /// </summary>
    public FlyTextFilterConfiguration()
    {
        this.Self = true;
        this.Enemy = true;
        this.Party = true;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FlyTextFilterConfiguration"/> class.
    /// </summary>
    /// <param name="toCopy">Configuration to copy from.</param>
    public FlyTextFilterConfiguration(FlyTextFilterConfiguration toCopy)
    {
        this.Self = toCopy.Self;
        this.Enemy = toCopy.Enemy;
        this.Party = toCopy.Party;
    }

    /// <summary>
    /// Gets or sets a value indicating whether this is enabled for self.
    /// </summary>
    public bool Self { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this is enabled for enemies.
    /// </summary>
    public bool Enemy { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this is enabled for part members.
    /// </summary>
    public bool Party { get; set; }
}