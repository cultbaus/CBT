namespace Scroll.FlyText;

using System.Numerics;

using FFXIVClientStructs.FFXIV.Client.Game.Character;
using ImGuiNET;

internal unsafe partial class FlyTextEvent
{
    internal FlyTextEvent(FlyTextKind kind, Character* target, Character* source, int option, int actionKind, int actionID, int val1, int val2, int val3, int val4)
    {
        FlyTextConfiguration config = Service.Configuration.FlyText[kind];

        this.Config = new FlyTextConfiguration(
            config.Font.Name,
            config.Font.Size,
            config.Font.Color,
            config.Font.Format,
            config.Outline.Size,
            config.Outline.Color,
            config.Animation.Kind,
            config.Animation.Duration,
            config.Animation.Speed);
        this.Animation = FlyTextAnimation.Create(kind);
        this.Target = target;
        this.Source = source;
        this.Option = option;
        this.ActionKind = actionKind;
        this.ActionID = actionID;
        this.Value1 = val1;
        this.Value2 = val2;
        this.Value3 = val3;
        this.Value4 = val4;
    }

    internal void Update(float timeElapsed)
    {
        this.Animation.TimeElapsed += timeElapsed;
        this.Animation.Apply(this, timeElapsed);
    }

    internal bool IsExpired
        => this.Animation.TimeElapsed > this.Animation.Duration;
    internal Vector2 Anchor
        => Service.GameGui.WorldToScreen(this.Target->Position, out Vector2 currentPosition) ? currentPosition : Vector2.Zero;
    internal Vector2 Position
        => this.Anchor + this.Animation.Offset;
    internal Vector2 Size
        => ImGui.CalcTextSize(this.Text);
    internal string Text
        => this.Config.Font.Format ? this.Value1.ToString("N0") : this.Value1.ToString();

    // Configuration
    internal FlyTextConfiguration Config { get; set; }

    // Animation
    internal FlyTextAnimation Animation { get; set; }

    // Arguments passed into the original hook
    internal Character* Target { get; set; }
    internal Character* Source { get; set; }
    internal int Option { get; set; }
    internal int ActionKind { get; set; }
    internal int ActionID { get; set; }
    internal int Value1 { get; set; }
    internal int Value2 { get; set; }
    internal int Value3 { get; set; }
    internal int Value4 { get; set; }
}