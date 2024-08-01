namespace CBT.FlyText.Types;

using System.Numerics;
using CBT.FlyText.Animations;
using CBT.FlyText.Configuration;
using Dalamud.Interface.Textures.TextureWraps;
using FFXIVClientStructs.FFXIV.Client.Game.Character;
using ImGuiNET;

/// <summary>
/// FlyTextEvent is a CBT FlyTextEvent wrapper over the in-game event.
/// </summary>
public unsafe partial class FlyTextEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FlyTextEvent"/> class.
    /// </summary>
    /// <param name="target">Target for the incoming FlyText event.</param>
    /// <param name="source">Source of the FlyText event.</param>
    /// <param name="kind">Kind of the FlyText event.</param>
    /// <param name="option">Option. Unused.</param>
    /// <param name="actionKind">ActionKind of the FlyText event.</param>
    /// <param name="actionID">ActionID of the FlyText event.</param>
    /// <param name="val1">Val1 of the FlyText event.</param>
    /// <param name="val2">Val2 of the FlyText event.</param>
    /// <param name="val3">Val3 of the FlyText event.</param>
    /// <param name="val4">Val4 of the FlyText event.</param>
    public FlyTextEvent(FlyTextKind kind, Character* target, Character* source, int option, int actionKind, int actionID, int val1, int val2, int val3, int val4)
    {
        FlyTextConfiguration config = Service.Configuration.FlyTextKinds[kind];

        this.Config = new FlyTextConfiguration(
            config.Font.Name,
            config.Font.Size,
            config.Font.Color,
            config.Font.Format,
            config.Outline.Enabled,
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

    /// <summary>
    /// gets a value indicating whether an event is expired.
    /// </summary>
    public bool IsExpired
        => this.Animation.TimeElapsed > this.Animation.Duration;

    /// <summary>
    /// Gets a value indicating the world anchor of the event.
    /// </summary>
    public Vector2 Anchor
        => Service.GameGui.WorldToScreen(this.Target->Position, out Vector2 currentPosition) ? currentPosition : Vector2.Zero;

    /// <summary>
    /// Gets the position of the FlyTextEvent with the animation offset applied.
    /// </summary>
    public Vector2 Position
        => this.Anchor + this.Animation.Offset;

    /// <summary>
    /// Gets the size of the FlyTextEvent rectangle.
    /// </summary>
    public Vector2 Size
        => ImGui.CalcTextSize(this.Text);

    /// <summary>
    /// Gets the string representation of the FlyTextEvent Value1.
    /// </summary>
    public string Text
        => this.Config.Font.Format ? this.Value1.ToString("N0") : this.Value1.ToString();

    /// <summary>
    /// Gets the Dalamud Texture Wrap for the ActionID.
    /// </summary>
    public IDalamudTextureWrap? Icon

        // FIXME @cultbaus: Icons are incorrect, this is getting ~some~ texture but it's not ~the right~ texture.
        => Service.TextureProvider.GetFromGameIcon(this.ActionID).GetWrapOrDefault();

    /// <summary>
    /// Gets or sets the FlyTextConfiguration.
    /// </summary>
    public FlyTextConfiguration Config { get; set; }

    /// <summary>
    /// Gets or sets the FlyTextAnimation.
    /// </summary>
    public FlyTextAnimation Animation { get; set; }

    /// <summary>
    /// Gets the Target from the original event.
    /// </summary>
    public Character* Target { get; }

    /// <summary>
    /// Gets the Source from the original event.
    /// </summary>
    public Character* Source { get; }

    /// <summary>
    /// Gets the Option from the original event.
    /// </summary>
    public int Option { get; }

    /// <summary>
    /// Gets the ActionKind from the original event.
    /// </summary>
    public int ActionKind { get; }

    /// <summary>
    /// Gets the ActionID from the original event.
    /// </summary>
    public int ActionID { get; }

    /// <summary>
    /// Gets the Value1 from the original event.
    /// </summary>
    public int Value1 { get; }

    /// <summary>
    /// Gets the Value2 from the original event.
    /// </summary>
    public int Value2 { get; }

    /// <summary>
    /// Gets the Value3 from the original event.
    /// </summary>
    public int Value3 { get; }

    /// <summary>
    /// Gets the Value4 from the original event.
    /// </summary>
    public int Value4 { get; }

    /// <summary>
    /// Update applies state changes to the FlyText event.
    /// </summary>
    /// <param name="timeElapsed">Time since the last frame.</param>
    public void Update(float timeElapsed)
    {
        this.Animation.TimeElapsed += timeElapsed;
        this.Animation.Apply(this, timeElapsed);
    }
}