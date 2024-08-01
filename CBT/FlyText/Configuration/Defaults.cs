namespace CBT.FlyText.Configuration;

using System.Numerics;
using CBT.FlyText.Animations;

/// <summary>
/// Defaults for configuration settings.
/// </summary>
public static class Defaults
{
    /// <summary>
    /// Default font.
    /// </summary>
    public const string DefaultFontName = "Expressway";

    /// <summary>
    /// Default font format options.
    /// </summary>
    public const bool DefaultFontFormat = true;

    /// <summary>
    /// Default font size.
    /// </summary>
    public const float DefaultFontSize = 18f;

    /// <summary>
    /// Default outline enabled.
    /// </summary>
    public const bool DefaultOutlineEnabled = true;

    /// <summary>
    /// Default outline thickness.
    /// </summary>
    public const int DefaultOutlineThickness = 1;

    /// <summary>
    /// Default animation kind.
    /// </summary>
    public const FlyTextAnimationKind DefaultAnimationKind = FlyTextAnimationKind.LinearFade;

    /// <summary>
    /// Default animation duration.
    /// </summary>
    public const float DefaultAnimationDuration = 3f;

    /// <summary>
    /// Default animation speed.
    /// </summary>
    public const float DefaultAnimationSpeed = 120f;

    /// <summary>
    /// Default font color.
    /// </summary>
    public static readonly Vector4 DefaultFontColor = Vector4.One;

    /// <summary>
    /// Default outline color.
    /// </summary>
    public static readonly Vector4 DefaultOutlineColor = new Vector4(0, 0, 0, 1);
}