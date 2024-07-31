namespace CBT.FlyText.Animations;

using System;
using System.Numerics;

using CBT.FlyText.Types;

/// <summary>
/// FlyTextAnimationKind enumerates the kinds of animations for <see cref="FlyTextEvent"/>s.
/// </summary>
internal enum FlyTextAnimationKind
{
    /// <summary>
    /// Animation no-op.
    /// </summary>
    None = 0,

    /// <summary>
    /// Default linear fade animation kind.
    /// </summary>
    LinearFade = 1,
}

/// <summary>
/// FlyTextAnimation defines the abstract class which must be implemented by Animations.
/// </summary>
internal abstract class FlyTextAnimation
{
    /// <summary>
    /// Gets the Animation duration.
    /// </summary>
    internal float Duration
        => PluginConfiguration.FlyText[this.FlyTextKind].Animation.Duration;

    /// <summary>
    /// Gets the Animation speed.
    /// </summary>
    internal float Speed
        => PluginConfiguration.FlyText[this.FlyTextKind].Animation.Speed;

    /// <summary>
    /// Gets or sets the FlyAnimationKind.
    /// </summary>
    internal FlyTextAnimationKind AnimationKind { get; set; }

    /// <summary>
    /// Gets or sets the FlyTextKind.
    /// </summary>
    internal FlyTextKind FlyTextKind { get; set; }

    /// <summary>
    /// Gets or sets the position offset.
    /// </summary>
    internal Vector2 Offset { get; set; }

    /// <summary>
    /// Gets or sets the Time Elapsed since created.
    /// </summary>
    internal float TimeElapsed { get; set; }

    /// <summary>
    /// Gets or sets the current Alpha.
    /// </summary>
    internal float Alpha { get; set; }

    /// <summary>
    /// Create an instance of a FlyTextAnimation type.
    /// </summary>
    /// <param name="flyTextKind">FlyTextKind determines the animation type.</param>
    /// <returns>FlyTextAnimation implementation.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Exception thrown when an invalid kind is received.</exception>
    internal static FlyTextAnimation Create(FlyTextKind flyTextKind)
    {
        FlyTextAnimationKind animationKind = PluginConfiguration.FlyText[flyTextKind].Animation.Kind;
        return animationKind switch
        {
            FlyTextAnimationKind.None => new None(flyTextKind),
            FlyTextAnimationKind.LinearFade => new LinearFade(flyTextKind),
            _ => throw new ArgumentOutOfRangeException(nameof(animationKind), animationKind, null),
        };
    }

    /// <summary>
    /// Apply calculates changes from the previous frame and updates the state of the <see cref="FlyTextEvent"/>.
    /// </summary>
    /// <param name="flyTextEvent">FlyTextEvent which is being animated.</param>
    /// <param name="timeSinceCreated">Time since the event was first drawn to the screen.</param>
    internal abstract void Apply(FlyTextEvent flyTextEvent, float timeSinceCreated);
}