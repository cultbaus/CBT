namespace Scroll.FlyText;

using Dalamud.Game.Gui.FlyText;
using Scroll.Flytext;

internal abstract class FlyTextAnimation
{
    public FlyTextAnimationKind AnimationKind { get; set; }

    public FlyTextKind FlyTextKind { get; set; }

    public float Duration
        => Service.Configuration.FlyText[this.FlyTextKind].Animation.Duration;

    public abstract void Apply(FlyTextEvent flyTextEvent, float timeSinceCreated);
}