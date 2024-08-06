namespace CBT.FlyText.Animations;

using CBT.Types;

/// <summary>
///  None is the No-op animation for <see cref="FlyTextEvent"/>s.
/// </summary>
public class None : FlyTextAnimation
{
    /// <summary>
    /// Initializes a new instance of the <see cref="None"/> class.
    /// </summary>
    /// <param name="kind">FlyTextKind for the Animation parent.</param>
    public None(FlyTextKind kind)
    {
        this.FlyTextKind = kind;
        this.AnimationKind = FlyTextAnimationKind.None;
    }

    /// <summary>
    /// Apply is a No-op for this type.
    /// </summary>
    /// <param name="flyTextEvent">FlyTextEvent which will not be animation.</param>
    /// <param name="timeSinceCreated">Time since the event was created.</param>
    public override void Apply(FlyTextEvent flyTextEvent, float timeSinceCreated)
    {
        // No-op
    }
}