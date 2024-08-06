namespace CBT;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using CBT.FlyText;
using CBT.FlyText.Configuration;
using CBT.Types;
using Dalamud.Configuration;

/// <summary>
/// Dalamud plugin configuration implementation.
/// </summary>
[Serializable]
public class PluginConfiguration : IPluginConfiguration
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PluginConfiguration"/> class.
    /// </summary>
    public PluginConfiguration()
    {
        this.FlyTextKinds = FlyTextKindExtension
            .GetAll()
            .ToDictionary(
                kind => kind,
                kind => new FlyTextConfiguration());

        this.FlyTextCategories = FlyTextCategoryExtension
            .GetAllCategories()
            .ToDictionary(
                kind => kind,
                kind => new FlyTextConfiguration());

        this.FlyTextGroups = FlyTextCategoryExtension
            .GetAllGroups()
            .ToDictionary(
                kind => kind,
                kind => new FlyTextConfiguration());

        FlyTextCategory.AbilityDamage
            .ForEach(kind =>
            {
                this.FlyTextKinds[kind].Font.Color = new Vector4(255 / 255, 255 / 255, 0, 255 / 255);
                this.FlyTextKinds[kind].Font.Size = 24f;
            });

        FlyTextCategory.AutoAttack
            .ForEach(kind =>
            {
                this.FlyTextKinds[kind].Font.Color = new Vector4(255 / 255, 255 / 255, 255 / 255, 255 / 255);
                this.FlyTextKinds[kind].Font.Size = 18f;
            });

        FlyTextCategory.AbilityHealing
            .ForEach(kind =>
            {
                this.FlyTextKinds[kind].Font.Color = new Vector4(0, 255 / 255, 0, 255 / 255);
                this.FlyTextKinds[kind].Font.Size = 18f;
            });

        FlyTextCategory.Miss
            .ForEach(kind =>
            {
                this.FlyTextKinds[kind].Font.Color = new Vector4(255 / 255, 255 / 4, 255 / 4, 255 / 255);
                this.FlyTextKinds[kind].Font.Size = 18f;
            });

        FlyTextCategory.NonCombat
            .ForEach(kind =>
            {
                this.FlyTextKinds[kind].Enabled = false;
            });

        FlyTextCategory.NonCombat
            .ForEach(kind =>
            {
                this.FlyTextKinds[kind].Enabled = false;
            });

        FlyTextCategory.Buff
            .Where(kind => kind == FlyTextKind.DebuffFading || kind == FlyTextKind.BuffFading)
            .ToList()
            .ForEach(kind =>
            {
                this.FlyTextKinds[kind].Animation.Reversed = true;
                this.FlyTextKinds[kind].Icon.Enabled = false;
            });
    }

    /// <summary>
    /// Gets or sets the Fonts configuration settings.
    /// </summary>
    public static Dictionary<string, List<float>> Fonts { get; set; } = new Dictionary<string, List<float>>();

    /// <summary>
    /// Gets or sets the FlyTextKinds Configuration options.
    /// </summary>
    public Dictionary<FlyTextKind, FlyTextConfiguration> FlyTextKinds { get; set; } = new Dictionary<FlyTextKind, FlyTextConfiguration>();

    /// <summary>
    /// Gets or sets the FlyTextCategory Category Configuration options.
    /// </summary>
    public Dictionary<FlyTextCategory, FlyTextConfiguration> FlyTextCategories { get; set; } = new Dictionary<FlyTextCategory, FlyTextConfiguration>();

    /// <summary>
    /// Gets or sets the FlyTextCategory Group Configuration options.
    /// </summary>
    public Dictionary<FlyTextCategory, FlyTextConfiguration> FlyTextGroups { get; set; } = new Dictionary<FlyTextCategory, FlyTextConfiguration>();

    /// <summary>
    /// Gets or sets a value indicating whether warnings are enabled.
    /// </summary>
    public bool Warnings { get; set; }

    /// <summary>
    /// Gets or sets the Configuration Version.
    /// </summary>
    public int Version { get; set; } = 0;

    /// <summary>
    /// Persist the configuration settings to disk.
    /// </summary>
    public void Save()
        => Service.Interface.SavePluginConfig(this);
}