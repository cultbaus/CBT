namespace Scroll.FlyText;

using System.Numerics;

internal class FlyTextConfiguration
{
    public FlyTextConfiguration( string fontName, float fontSize, Vector4 fontColor, float outlineSize, Vector4 outlineColor, FlyTextAnimationKind animationKind, float animationDuration)
    {
        this.Font = new FlyTextFontConfiguration(fontSize, fontName, fontColor);
        this.Outline = new FlyTextOutlineConfiguration(outlineSize, outlineColor);
        this.Animation = new FlyTextAnimationConfiguration(animationKind, animationDuration);
    }

    public bool Enabled { get; set; } = true;

    public FlyTextFontConfiguration Font { get; set; }

    public FlyTextOutlineConfiguration Outline { get; set; }

    public FlyTextAnimationConfiguration Animation { get; set; }

}

internal class FlyTextFontConfiguration
{
    internal FlyTextFontConfiguration(float size, string name, Vector4 color)
    {
        this.Size = size;
        this.Name = name;
        this.Color = color;
    }

    public float Size { get; set; }

    public string Name { get; set; }

    public Vector4 Color { get; set; }
}

internal class FlyTextOutlineConfiguration
{
    internal FlyTextOutlineConfiguration(float size, Vector4 color)
    {
        this.Enabled = true;
        this.Size = size;
        this.Color = color;
    }

    public bool Enabled { get; set; }

    public float Size { get; set; }

    public Vector4 Color { get; set; }
}

internal class FlyTextAnimationConfiguration
{
    internal FlyTextAnimationConfiguration(FlyTextAnimationKind kind, float duration)
    {
        this.Kind = kind;
        this.Duration = duration;
    }

    public FlyTextAnimationKind Kind { get; set; }

    public float Duration { get; set; }
}