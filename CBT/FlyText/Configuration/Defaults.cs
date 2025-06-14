namespace CBT.FlyText.Configuration;

using System.Numerics;
using CBT.FlyText.Animations;
using Dalamud.Interface.FontIdentifier;
using Dalamud.Interface.GameFonts;

/// <summary>
/// Defaults for configuration settings.
/// </summary>
public record Defaults
{
    /// <summary>
    /// Default font.
    /// </summary>
    public static readonly IFontId DefaultFontId = new GameFontAndFamilyId(GameFontFamily.Axis);

    /// <summary>
    /// Default message format option.
    /// </summary>
    public const bool DefaultMessageFormat = true;

    /// <summary>
    /// Default font size.
    /// </summary>
    public const float DefaultFontSize = 18f;

    /// <summary>
    /// Default outline enabled.
    /// </summary>
    public const bool DefaultOutlineEnabled = true;

    /// <summary>
    /// Default outline enabled.
    /// </summary>
    public const bool DefaultIconEnabled = true;

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
    /// Default icon zoom.
    /// </summary>
    public const float DefaultIconZoom = 0.7f;

    /// <summary>
    /// Default outline reversed.
    /// </summary>
    public const bool DefaultAnimationReversed = false;

    /// <summary>
    /// Default outline reversed.
    /// </summary>
    public const FlyTextAlignment DefaultAnimationAlignment = FlyTextAlignment.Left;

    /// <summary>
    /// Default font color.
    /// </summary>
    public static readonly Vector4 DefaultFontColor = Vector4.One;

    /// <summary>
    /// Default outline color.
    /// </summary>
    public static readonly Vector4 DefaultOutlineColor = new Vector4(0, 0, 0, 1);

    /// <summary>
    /// Default icon size.
    /// </summary>
    public static readonly Vector2 DefaultIconSize = new Vector2(DefaultFontSize, DefaultFontSize);

    /// <summary>
    /// Default icon offset.
    /// </summary>
    public static readonly Vector2 DefaultIconOffset = new Vector2(-10, 0);
}