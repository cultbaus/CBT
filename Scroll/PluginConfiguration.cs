namespace Scroll;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

using Dalamud.Configuration;

using Scroll.FlyText;
using Scroll.FlyText.Types;

internal class PluginConfiguration : IPluginConfiguration
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
            this.DefaultFontFormat,
            this.DefaultOutlineThickness,
            this.DefaultOutlineColor,
            this.DefaultAnimationKind,
            this.DefaultAnimationDuration,
            this.DefaultAnimationSpeed));
    }

    internal void Save()
        => Service.Interface.SavePluginConfig(this);

    // Configuration
    public int Version { get; set; } = 0;
    internal Dictionary<FlyTextKind, FlyTextConfiguration> FlyText { get; set; } = new Dictionary<FlyTextKind, FlyTextConfiguration>();
    internal Dictionary<string, List<float>> Fonts { get; set; } = new Dictionary<string, List<float>>();

    // Font defaults
    internal string DefaultFont { get; private set; } = "Expressway";
    internal float DefaultFontSize { get; private set; } = 18f;
    internal Vector4 DefaultFontColor { get; private set; } = Vector4.One;
    internal bool DefaultFontFormat { get; private set; } = true;

    // Outline defaults
    internal bool DefaultOutline { get; private set; } = true;
    internal int DefaultOutlineThickness { get; private set; } = 1;
    internal Vector4 DefaultOutlineColor { get; private set; } = new Vector4(0, 0, 0, 1);

    // Animation defaults
    internal FlyTextAnimationKind DefaultAnimationKind { get; private set; } = FlyTextAnimationKind.LinearFade;
    internal float DefaultAnimationDuration { get; private set; } = 5f;
    internal float DefaultAnimationSpeed { get; private set; } = 120f;
}