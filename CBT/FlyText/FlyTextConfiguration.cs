namespace CBT.FlyText;

using System.Numerics;

internal class FlyTextConfiguration
{
    internal FlyTextConfiguration()
    {
        this.Font = new FlyTextFontConfiguration(Defaults.DefaultFontSize, Defaults.DefaultFontName, Defaults.DefaultFontColor, Defaults.DefaultFontFormat);
        this.Outline = new FlyTextOutlineConfiguration(Defaults.DefaultOutlineThickness, Defaults.DefaultOutlineColor);
        this.Animation = new FlyTextAnimationConfiguration(Defaults.DefaultAnimationKind, Defaults.DefaultAnimationDuration, Defaults.DefaultAnimationSpeed);
    }

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

internal static class Defaults
{
    // Font defaults
    internal const string DefaultFontName = "Expressway";
    internal const float DefaultFontSize = 18f;
    internal static readonly Vector4 DefaultFontColor = Vector4.One;
    internal const bool DefaultFontFormat = true;

    // Outline defaults
    internal const bool DefaultOutlineEnabled = true;
    internal const int DefaultOutlineThickness = 1;
    internal static readonly Vector4 DefaultOutlineColor = new Vector4(0, 0, 0, 1);

    // Animation defaults
    internal const FlyTextAnimationKind DefaultAnimationKind = FlyTextAnimationKind.LinearFade;
    internal const float DefaultAnimationDuration = 3f;
    internal const float DefaultAnimationSpeed = 120f;
}