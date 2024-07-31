namespace CBT;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

using Dalamud.Configuration;

using CBT.FlyText;
using CBT.FlyText.Types;

internal class PluginConfiguration : IPluginConfiguration
{
    internal PluginConfiguration()
    {
        this.FlyText = Enum.GetValues<FlyTextKind>()
        .Cast<FlyTextKind>()
        .ToDictionary(
            kind => kind,
            kind => new FlyTextConfiguration());

        FlyTextCategory.AbilityDamage
            .ForEach(kind =>
            {
                this.FlyText[kind].Font.Color = new Vector4(255 / 255, 255 / 255, 0, 255 / 255);
                this.FlyText[kind].Font.Size = 24f;
            });

        FlyTextCategory.AutoAttack
            .ForEach(kind =>
            {
                this.FlyText[kind].Font.Color = new Vector4(255 / 255, 255 / 255, 255 / 255, 255 / 255);
                this.FlyText[kind].Font.Size = 18f;
            });

        FlyTextCategory.AbilityHealing
            .ForEach(kind =>
            {
                this.FlyText[kind].Font.Color = new Vector4(0, 255 / 255, 0, 255 / 255);
                this.FlyText[kind].Font.Size = 18f;
            });

        FlyTextCategory.Miss
            .ForEach(kind =>
            {
                this.FlyText[kind].Font.Color = new Vector4(255 / 255, 255 / 4, 255 / 4, 255 / 255);
                this.FlyText[kind].Font.Size = 18f;
            });

        FlyTextCategory.NonCombat
            .ForEach(kind =>
            {
                this.FlyText[kind].Enabled = false;
            });
    }

    internal void Save()
        => Service.Interface.SavePluginConfig(this);

    // Configuration
    public int Version { get; set; } = 0;
    internal Dictionary<FlyTextKind, FlyTextConfiguration> FlyText { get; set; } = new Dictionary<FlyTextKind, FlyTextConfiguration>();
    internal Dictionary<string, List<float>> Fonts { get; set; } = new Dictionary<string, List<float>>();
}