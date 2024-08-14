namespace CBT.FlyText.Animations;

using System;
using System.Numerics;

using CBT.Types;

/// <summary>
/// FlyTextAnimationKind enumerates the kinds of animations for <see cref="FlyTextEvent"/>s.
/// </summary>
public enum FlyTextAnimationKind
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
/// Alignment for fly text animations.
/// </summary>
public enum FlyTextAlignment
{
    /// <summary>
    /// Alignment no-op.
    /// </summary>
    None = 0,

    /// <summary>
    /// Center alignment.
    /// </summary>
    Center = 1,

    /// <summary>
    /// Left alignment.
    /// </summary>
    Left = 2,

    /// <summary>
    /// Right alignment.
    /// </summary>
    Right = 3,
}

/// <summary>
/// FlyTextAnimation defines the abstract class which must be implemented by Animations.
/// </summary>
public abstract class FlyTextAnimation
{
    /// <summary>
    /// Gets the Animation duration.
    /// </summary>
    public float Duration
        => Service.Configuration.FlyTextKinds[this.FlyTextKind].Animation.Duration;

    /// <summary>
    /// Gets the Animation speed.
    /// </summary>
    public float Speed
        => Service.Configuration.FlyTextKinds[this.FlyTextKind].Animation.Speed;

    /// <summary>
    /// Gets a value indicating whether the animation direction should be reversed.
    /// </summary>
    public bool Reversed
        => Service.Configuration.FlyTextKinds[this.FlyTextKind].Animation.Reversed;

    /// <summary>
    /// Gets the alignment of an element.
    /// </summary>
    public FlyTextAlignment Alignment
        => Service.Configuration.FlyTextKinds[this.FlyTextKind].Animation.Alignment;

    /// <summary>
    /// Gets or sets the FlyAnimationKind.
    /// </summary>
    public FlyTextAnimationKind AnimationKind { get; set; }

    /// <summary>
    /// Gets or sets the FlyTextKind.
    /// </summary>
    public FlyTextKind FlyTextKind { get; set; }

    /// <summary>
    /// Gets or sets the position offset.
    /// </summary>
    public Vector2 Offset { get; set; }

    /// <summary>
    /// Gets or sets the Time Elapsed since created.
    /// </summary>
    public float TimeElapsed { get; set; }

    /// <summary>
    /// Gets or sets the current Alpha.
    /// </summary>
    public float Alpha { get; set; }

    /// <summary>
    /// Center align an element.
    /// </summary>
    /// <param name="flyTextEvent">The event.</param>
    /// <returns>The new position after adjusting.</returns>
    public static Vector2 Center(FlyTextEvent flyTextEvent)
    {
        return new(flyTextEvent.Position.X - (flyTextEvent.Size.X / 2), flyTextEvent.Position.Y - (flyTextEvent.Size.Y / 2));
    }

    /// <summary>
    /// Left align an element.
    /// </summary>
    /// <param name="flyTextEvent">The event.</param>
    /// <returns>The new position after adjusting.</returns>
    public static Vector2 Left(FlyTextEvent flyTextEvent)
    {
        return flyTextEvent.Position;
    }

    /// <summary>
    /// Create an instance of a FlyTextAnimation type.
    /// </summary>
    /// <param name="flyTextKind">FlyTextKind determines the animation type.</param>
    /// <returns>FlyTextAnimation implementation.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Exception thrown when an invalid kind is received.</exception>
    public static FlyTextAnimation Create(FlyTextKind flyTextKind)
    {
        FlyTextAnimationKind animationKind = Service.Configuration.FlyTextKinds[flyTextKind].Animation.Kind;
        return animationKind switch
        {
            FlyTextAnimationKind.None => new None(flyTextKind),
            FlyTextAnimationKind.LinearFade => new LinearFade(flyTextKind),
            _ => throw new ArgumentOutOfRangeException(nameof(flyTextKind), animationKind, null),
        };
    }

    /// <summary>
    /// Apply calculates changes from the previous frame and updates the state of the <see cref="FlyTextEvent"/>.
    /// </summary>
    /// <param name="flyTextEvent">FlyTextEvent which is being animated.</param>
    /// <param name="timeSinceCreated">Time since the event was first drawn to the screen.</param>
    public abstract void Apply(FlyTextEvent flyTextEvent, float timeSinceCreated);

    /// <summary>
    /// Align the element.
    /// </summary>
    /// <param name="e">Event to align.</param>
    /// <returns>The adjusted position of the element.</returns>
    public Vector2 Align(FlyTextEvent e) => this.Alignment switch
    {
        FlyTextAlignment.Center => Center(e),
        FlyTextAlignment.Left => Left(e),
        FlyTextAlignment.Right => throw new NotImplementedException(nameof(this.Alignment)),
        _ => throw new ArgumentOutOfRangeException(nameof(e), this.Alignment, null),
    };
}