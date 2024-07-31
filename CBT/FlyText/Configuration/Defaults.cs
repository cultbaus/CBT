namespace CBT.FlyText.Configuration;

using System.Numerics;
using CBT.FlyText.Animations;

/// <summary>
/// Defaults for configuration settings.
/// </summary>
internal static class Defaults
{
    /// <summary>
    /// Default font.
    /// </summary>
    internal const string DefaultFontName = "Expressway";

    /// <summary>
    /// Default font format options.
    /// </summary>
    internal const bool DefaultFontFormat = true;

    /// <summary>
    /// Default font size.
    /// </summary>
    internal const float DefaultFontSize = 18f;

    /// <summary>
    /// Default outline enabled.
    /// </summary>
    internal const bool DefaultOutlineEnabled = true;

    /// <summary>
    /// Default outline thickness.
    /// </summary>
    internal const int DefaultOutlineThickness = 1;

    /// <summary>
    /// Default animation kind.
    /// </summary>
    internal const FlyTextAnimationKind DefaultAnimationKind = FlyTextAnimationKind.LinearFade;

    /// <summary>
    /// Default animation duration.
    /// </summary>
    internal const float DefaultAnimationDuration = 3f;

    /// <summary>
    /// Default animation speed.
    /// </summary>
    internal const float DefaultAnimationSpeed = 120f;

    /// <summary>
    /// Default font color.
    /// </summary>
    internal static readonly Vector4 DefaultFontColor = Vector4.One;

    /// <summary>
    /// Default outline color.
    /// </summary>
    internal static readonly Vector4 DefaultOutlineColor = new Vector4(0, 0, 0, 1);
}