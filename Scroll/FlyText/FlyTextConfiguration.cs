namespace Scroll.FlyText;

using System.Numerics;

internal class FlyTextConfiguration
{
    internal FlyTextConfiguration(string fontName, float fontSize, Vector4 fontColor, bool fontFormat, int outlineSize, Vector4 outlineColor, FlyTextAnimationKind animationKind, float animationDuration, float animationSpeed)
    {
        this.Font = new FlyTextFontConfiguration(fontSize, fontName, fontColor, fontFormat);
        this.Outline = new FlyTextOutlineConfiguration(outlineSize, outlineColor);
        this.Animation = new FlyTextAnimationConfiguration(animationKind, animationDuration, animationSpeed);
    }

    internal bool Enabled { get; set; } = true;
    internal FlyTextFontConfiguration Font { get; set; }
    internal FlyTextOutlineConfiguration Outline { get; set; }
    internal FlyTextAnimationConfiguration Animation { get; set; }

}

internal class FlyTextFontConfiguration
{
    internal FlyTextFontConfiguration(float size, string name, Vector4 color, bool format)
    {
        this.Size = size;
        this.Name = name;
        this.Color = color;
        this.Format = format;
    }

    internal float Size { get; set; }
    internal string Name { get; set; }
    internal Vector4 Color { get; set; }
    internal bool Format { get; set; }
}

internal class FlyTextOutlineConfiguration
{
    internal FlyTextOutlineConfiguration(int size, Vector4 color)
    {
        this.Enabled = true;
        this.Size = size;
        this.Color = color;
    }

    internal bool Enabled { get; set; }
    internal int Size { get; set; }
    internal Vector4 Color { get; set; }
}

internal class FlyTextAnimationConfiguration
{
    internal FlyTextAnimationConfiguration(FlyTextAnimationKind kind, float duration, float speed)
    {
        this.Kind = kind;
        this.Duration = duration;
        this.Speed = speed;
    }

    internal FlyTextAnimationKind Kind { get; set; }
    internal float Duration { get; set; }
    internal float Speed { get; set; }
}