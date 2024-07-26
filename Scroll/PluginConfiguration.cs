namespace Scroll;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Dalamud.Configuration;
using Dalamud.Game.Gui.FlyText;

using Scroll.FlyText;

internal partial class PluginConfiguration : IPluginConfiguration
{
    internal PluginConfiguration()
    {
        this.FlyText = Enum.GetValues<FlyTextKind>()
        .Cast<FlyTextKind>()
        .ToDictionary(
            kind => kind,
            kind => new FlyTextConfiguration(
            this.DefaultFont,
            this.DefaultFontSize,
            this.DefaultFontColor,
            this.DefaultOutlineThickness,
            this.DefaultOutlineColor,
            this.DefaultAnimationKind,
            this.DefaultAnimationDuration));
    }

    public int Version { get; set; } = 0;

    public Dictionary<FlyTextKind, FlyTextConfiguration> FlyText { get; set; } = new Dictionary<FlyTextKind, FlyTextConfiguration>();

    public void Save()
        => Service.Interface.SavePluginConfig(this);
}

internal partial class PluginConfiguration
{
    public string DefaultFont { get; set; } = "Expressway";

    public float DefaultFontSize { get; set; } = 18f;

    public Vector4 DefaultFontColor { get; set; } = Vector4.One;

    public bool DefaultOutline { get; set; } = true;

    public float DefaultOutlineThickness { get; set; } = 1f;

    public Vector4 DefaultOutlineColor { get; set; } = new Vector4(0, 0, 0, 1);

    public FlyTextAnimationKind DefaultAnimationKind { get; set; } = FlyTextAnimationKind.LinearFade;

    public float DefaultAnimationDuration { get; set; } = 5f;
}