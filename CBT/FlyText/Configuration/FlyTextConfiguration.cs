namespace CBT.FlyText.Configuration;

using System.Numerics;
using CBT.FlyText.Animations;

/// <summary>
/// FlyText configuration options.
/// </summary>
internal class FlyTextConfiguration
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FlyTextConfiguration"/> class.
    /// </summary>
    internal FlyTextConfiguration()
    {
        this.Font = new FlyTextFontConfiguration(Defaults.DefaultFontSize, Defaults.DefaultFontName, Defaults.DefaultFontColor, Defaults.DefaultFontFormat);
        this.Outline = new FlyTextOutlineConfiguration(Defaults.DefaultOutlineEnabled, Defaults.DefaultOutlineThickness, Defaults.DefaultOutlineColor);
        this.Animation = new FlyTextAnimationConfiguration(Defaults.DefaultAnimationKind, Defaults.DefaultAnimationDuration, Defaults.DefaultAnimationSpeed);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FlyTextConfiguration"/> class.
    /// </summary>
    /// <param name="fontName">Font name.</param>
    /// <param name="fontSize">Font size.</param>
    /// <param name="fontColor">Font color.</param>
    /// <param name="fontFormat">Whether or not to Format the value.</param>
    /// <param name="outlineEnabled">Outline is enabled or disabled.</param>
    /// <param name="outlineSize">Outline size.</param>
    /// <param name="outlineColor">Outline color.</param>
    /// <param name="animationKind">Animation type.</param>
    /// <param name="animationDuration">Animation duration.</param>
    /// <param name="animationSpeed">Animation speed.</param>
    internal FlyTextConfiguration(string fontName, float fontSize, Vector4 fontColor, bool fontFormat, bool outlineEnabled, int outlineSize, Vector4 outlineColor, FlyTextAnimationKind animationKind, float animationDuration, float animationSpeed)
    {
        this.Font = new FlyTextFontConfiguration(fontSize, fontName, fontColor, fontFormat);
        this.Outline = new FlyTextOutlineConfiguration(outlineEnabled, outlineSize, outlineColor);
        this.Animation = new FlyTextAnimationConfiguration(animationKind, animationDuration, animationSpeed);
    }

    /// <summary>
    /// Gets or sets a value indicating whether or not the kind is enabled.
    /// </summary>
    internal bool Enabled { get; set; } = true;

    /// <summary>
    /// Gets or sets the font configuration.
    /// </summary>
    internal FlyTextFontConfiguration Font { get; set; }

    /// <summary>
    /// Gets or sets the outline configuration.
    /// </summary>
    internal FlyTextOutlineConfiguration Outline { get; set; }

    /// <summary>
    /// Gets or sets the animation configuration.
    /// </summary>
    internal FlyTextAnimationConfiguration Animation { get; set; }
}