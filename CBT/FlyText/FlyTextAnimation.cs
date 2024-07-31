namespace CBT.FlyText;

using System;
using System.Numerics;

using CBT.FlyText.Types;

internal enum FlyTextAnimationKind
{
    None = 0,
    LinearFade = 1,
}

internal abstract class FlyTextAnimation
{
    internal abstract void Apply(FlyTextEvent flyTextEvent, float timeSinceCreated);

    internal static FlyTextAnimation Create(FlyTextKind flyTextKind)
    {
        FlyTextAnimationKind animationKind = Service.Configuration.FlyText[flyTextKind].Animation.Kind;
        return animationKind switch
        {
            FlyTextAnimationKind.None => new None(flyTextKind),
            FlyTextAnimationKind.LinearFade => new LinearFade(flyTextKind),
            _ => throw new ArgumentOutOfRangeException(nameof(animationKind), animationKind, null),
        };
    }

    internal float Duration
        => Service.Configuration.FlyText[this.FlyTextKind].Animation.Duration;
    internal float Speed
        => Service.Configuration.FlyText[this.FlyTextKind].Animation.Speed;

    internal FlyTextAnimationKind AnimationKind { get; set; }
    internal FlyTextKind FlyTextKind { get; set; }
    internal Vector2 Offset { get; set; }
    internal float TimeElapsed { get; set; }
    internal float Alpha { get; set; }

}

internal class LinearFade : FlyTextAnimation
{
    internal LinearFade(FlyTextKind kind)
    {
        this.FlyTextKind = kind;
        this.AnimationKind = FlyTextAnimationKind.LinearFade;
    }

    internal override void Apply(FlyTextEvent flyTextEvent, float timeSinceCreated)
    {
        float yOffset = this.Offset.Y - this.Speed * timeSinceCreated;
        float alpha = Math.Max(0.0f, 1.0f - Math.Min(this.TimeElapsed / this.Duration, 1.0f));

        this.Offset = this.Offset with { Y = yOffset };
        this.Alpha = alpha;
    }
}

internal class None : FlyTextAnimation
{
    internal None(FlyTextKind kind)
    {
        this.FlyTextKind = kind;
        this.AnimationKind = FlyTextAnimationKind.None;
    }

    internal override void Apply(FlyTextEvent flyTextEvent, float timeSinceCreated) { }
}