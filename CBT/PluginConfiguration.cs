namespace CBT;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using CBT.FlyText;
using CBT.FlyText.Configuration;
using CBT.FlyText.Types;
using Dalamud.Configuration;

/// <summary>
/// Dalamud plugin configuration implementation.
/// </summary>
internal class PluginConfiguration : IPluginConfiguration
{
    /// <summary>
    /// Initializes static members of the <see cref="PluginConfiguration"/> class.
    /// Initializes a new isntance of the <see cref="PluginConfiguration"/> class.
    /// </summary>
    static PluginConfiguration()
    {
        FlyText = Enum.GetValues<FlyTextKind>()
        .Cast<FlyTextKind>()
        .ToDictionary(
            kind => kind,
            kind => new FlyTextConfiguration());

        FlyTextCategory.AbilityDamage
            .ForEach(kind =>
            {
                FlyText[kind].Font.Color = new Vector4(255 / 255, 255 / 255, 0, 255 / 255);
                FlyText[kind].Font.Size = 24f;
            });

        FlyTextCategory.AutoAttack
            .ForEach(kind =>
            {
                FlyText[kind].Font.Color = new Vector4(255 / 255, 255 / 255, 255 / 255, 255 / 255);
                FlyText[kind].Font.Size = 18f;
            });

        FlyTextCategory.AbilityHealing
            .ForEach(kind =>
            {
                FlyText[kind].Font.Color = new Vector4(0, 255 / 255, 0, 255 / 255);
                FlyText[kind].Font.Size = 18f;
            });

        FlyTextCategory.Miss
            .ForEach(kind =>
            {
                FlyText[kind].Font.Color = new Vector4(255 / 255, 255 / 4, 255 / 4, 255 / 255);
                FlyText[kind].Font.Size = 18f;
            });

        FlyTextCategory.NonCombat
            .ForEach(kind =>
            {
                FlyText[kind].Enabled = false;
            });
    }

    /// <summary>
    /// Gets or sets the Configuration Version.
    /// </summary>
    public int Version { get; set; } = 0;

    /// <summary>
    /// Gets or sets the FlyText Configuration options.
    /// </summary>
    internal static Dictionary<FlyTextKind, FlyTextConfiguration> FlyText { get; set; } = new Dictionary<FlyTextKind, FlyTextConfiguration>();

    /// <summary>
    /// Gets or sets the Fonts configuration settings.
    /// </summary>
    internal static Dictionary<string, List<float>> Fonts { get; set; } = new Dictionary<string, List<float>>();

    /// <summary>
    /// Persist the configuration settings to disk.
    /// </summary>
    internal void Save()
        => Service.Interface.SavePluginConfig(this);
}