namespace CBT.FlyText.Configuration;

using CBT.FlyText.Animations;

/// <summary>
/// FlyTextAnimation configuration.
/// </summary>
public class FlyTextAnimationConfiguration
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FlyTextAnimationConfiguration"/> class.
    /// </summary>
    public FlyTextAnimationConfiguration()
    {
        this.Kind = Defaults.DefaultAnimationKind;
        this.Duration = Defaults.DefaultAnimationDuration;
        this.Speed = Defaults.DefaultAnimationSpeed;
        this.Reversed = Defaults.DefaultAnimationReversed;
        this.Alignment = Defaults.DefaultAnimationAlignment;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FlyTextAnimationConfiguration"/> class.
    /// </summary>
    /// <param name="toCopy">Values to copy.</param>
    public FlyTextAnimationConfiguration(FlyTextAnimationConfiguration toCopy)
    {
        this.Kind = toCopy.Kind;
        this.Duration = toCopy.Duration;
        this.Speed = toCopy.Speed;
        this.Reversed = toCopy.Reversed;
        this.Alignment = toCopy.Alignment;
    }

    /// <summary>
    /// Gets or sets the animation kind.
    /// </summary>
    public FlyTextAnimationKind Kind { get; set; }

    /// <summary>
    /// Gets or sets the duration.
    /// </summary>
    public float Duration { get; set; }

    /// <summary>
    /// Gets or sets the speed.
    /// </summary>
    public float Speed { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the animation axis should be reversed.
    /// </summary>
    public bool Reversed { get; set; }

    /// <summary>
    /// Gets or sets the alignment.
    /// </summary>
    public FlyTextAlignment Alignment { get; set; }
}