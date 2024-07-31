namespace CBT.FlyText.Animations;

using System;
using CBT.FlyText.Types;

/// <summary>
/// LinearFade is the default animation style for <see cref="FlyTextEvent"/>s.
/// </summary>
internal class LinearFade : FlyTextAnimation
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LinearFade"/> class.
    /// </summary>
    /// <param name="kind">FlyTextKind of the Animation parent.</param>
    internal LinearFade(FlyTextKind kind)
    {
        this.FlyTextKind = kind;
        this.AnimationKind = FlyTextAnimationKind.LinearFade;
    }

    /// <inheritdoc/>
    internal override void Apply(FlyTextEvent flyTextEvent, float timeSinceCreated)
    {
        float yOffset = this.Offset.Y - (this.Speed * timeSinceCreated);
        float alpha = Math.Max(0.0f, 1.0f - Math.Min(this.TimeElapsed / this.Duration, 1.0f));

        this.Offset = this.Offset with { Y = yOffset };
        this.Alpha = alpha;
    }
}