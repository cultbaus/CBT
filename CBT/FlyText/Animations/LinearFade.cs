namespace CBT.FlyText.Animations;

using System;
using CBT.Types;

/// <summary>
/// LinearFade is the default animation style for <see cref="FlyTextEvent"/>s.
/// </summary>
public class LinearFade : FlyTextAnimation
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LinearFade"/> class.
    /// </summary>
    /// <param name="kind">FlyTextKind of the Animation parent.</param>
    public LinearFade(FlyTextKind kind)
    {
        this.FlyTextKind = kind;
        this.AnimationKind = FlyTextAnimationKind.LinearFade;
    }

    /// <inheritdoc/>
    public override void Apply(FlyTextEvent flyTextEvent, float timeSinceCreated)
    {
        this.Offset = this.Offset with { Y = this.GetDirection(flyTextEvent, timeSinceCreated) };
        this.Alpha = Math.Max(0.0f, 1.0f - Math.Min(this.TimeElapsed / this.Duration, 1.0f));
    }

    private float GetDirection(FlyTextEvent flyTextEvent, float timeSinceCreated)
        => flyTextEvent.Animation.Reversed
               ? this.Offset.Y + (this.Speed * timeSinceCreated)
               : this.Offset.Y - (this.Speed * timeSinceCreated);
}