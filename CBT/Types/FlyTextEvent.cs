namespace CBT.Types;

using System;
using System.Linq;
using System.Numerics;
using CBT.FlyText.Animations;
using CBT.FlyText.Configuration;
using Dalamud.Interface.Textures;
using Dalamud.Interface.Textures.TextureWraps;
using FFXIVClientStructs.FFXIV.Client.Game.Character;
using ImGuiNET;
using static FFXIVClientStructs.FFXIV.Client.Game.Character.ActionEffectHandler;

/// <summary>
/// Initializes a new instance of the <see cref="FlyTextEvent"/> class.
/// <param name="kind">Kind of the FlyText event.</param>
/// <param name="effects">An array of effects associated with this event.</param>
/// <param name="target">Target for the incoming FlyText event.</param>
/// <param name="source">Source of the FlyText event.</param>
/// <param name="option">Option. Unused.</param>
/// <param name="actionKind">ActionKind of the FlyText event.</param>
/// <param name="actionID">ActionID of the FlyText event.</param>
/// <param name="val1">Val1 of the FlyText event.</param>
/// <param name="val2">Val2 of the FlyText event.</param>
/// <param name="val3">Val3 of the FlyText event.</param>
/// <param name="val4">Val4 of the FlyText event.</param>
/// </summary>
public unsafe partial class FlyTextEvent(FlyTextKind kind, Effect[] effects, Character* target, Character* source, int option, int actionKind, int actionID, int val1, int val2, int val3, int val4)
{
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
    {
        // FIXME @cultbaus: This needs to get the font size to correctly calculate the size of the text.
        // I don't know the performance implications of doing it this way.
        get
        {
            var fontConfig = Service.Configuration.FlyTextKinds[this.Kind].Font;
            using (Service.Fonts.Push(fontConfig.Name, fontConfig.Size))
            {
                var textSize = ImGui.CalcTextSize(this.Text);
                var iconSize = this.Icon?.Size ?? Vector2.Zero;

                var totalWidth = Math.Max(textSize.X, iconSize.X + textSize.X);
                var totalHeight = Math.Max(textSize.Y, iconSize.Y);

                return new Vector2(totalWidth, totalHeight);
            }
        }
    }

    /// <summary>
    /// Gets the string representation of the FlyTextEvent Value1.
    /// </summary>
    public string Text
        => this.Kind.IsStatus() ?
            this.Format(this.Name) : this.Format(this.Value1.ToString("N0"));

    /// <summary>
    /// Gets the Dalamud Texture Wrap for the ActionID.
    /// </summary>
    public IDalamudTextureWrap? Icon
        => Service.TextureProvider.GetFromGameIcon(new GameIconLookup((uint)this.IconID, false, true)).GetWrapOrDefault();

    /// <summary>
    /// Gets the IconID for the provided Action.
    /// </summary>
    public ushort IconID
        => this.Kind.IsStatus()
            ? Service.Ability.GetIconForStatus(this.Value1)
            : Service.Ability.GetIconForAction(this.ActionID);

    /// <summary>
    /// Gets the Name for the provided Action.
    /// </summary>
    public string Name
        => this.Kind.IsStatus()
            ? Service.Ability.GetNameForStatus(this.Value1)
            : Service.Ability.GetNameForAction(this.ActionID);

    /// <summary>
    /// Gets the damage type of an action.
    /// </summary>
    public DamageKind? DamageKind
        => this.Kind.InCategory(FlyTextCategory.AbilityDamage)
            ? (DamageKind)(this.Effects
                .ToList()
                .Where(effect => (ActionEventKind)effect.Type == ActionEventKind.Damage)
                .FirstOrDefault(default(Effect))
            .Param1 & 0xF)
            : null;

    /// <summary>
    /// Gets the fly text kind.
    /// </summary>
    public FlyTextKind Kind { get; private set; } = kind;

    /// <summary>
    /// Gets or sets the FlyTextConfiguration.
    /// </summary>
    public FlyTextConfiguration Config { get; set; } = new FlyTextConfiguration(kind);

    /// <summary>
    /// Gets or sets the FlyTextAnimation.
    /// </summary>
    public FlyTextAnimation Animation { get; set; } = FlyTextAnimation.Create(kind);

    /// <summary>
    /// Gets or sets the Effects associated with the event.
    /// </summary>
    public Effect[] Effects { get; set; } = effects;

    /// <summary>
    /// Gets the Target from the original event.
    /// </summary>
    public Character* Target { get; } = target;

    /// <summary>
    /// Gets the Source from the original event.
    /// </summary>
    public Character* Source { get; } = source;

    /// <summary>
    /// Gets the Option from the original event.
    /// </summary>
    public int Option { get; } = option;

    /// <summary>
    /// Gets the ActionKind from the original event.
    /// </summary>
    public int ActionKind { get; } = actionKind;

    /// <summary>
    /// Gets the ActionID from the original event.
    /// </summary>
    public int ActionID { get; } = actionID;

    /// <summary>
    /// Gets the Value1 from the original event.
    /// </summary>
    public int Value1 { get; } = val1;

    /// <summary>
    /// Gets the Value2 from the original event.
    /// </summary>
    public int Value2 { get; } = val2;

    /// <summary>
    /// Gets the Value3 from the original event.
    /// </summary>
    public int Value3 { get; } = val3;

    /// <summary>
    /// Gets the Value4 from the original event.
    /// </summary>
    public int Value4 { get; } = val4;

    /// <summary>
    /// Update applies state changes to the FlyText event.
    /// </summary>
    /// <param name="timeElapsed">Time since the last frame.</param>
    public void Update(float timeElapsed)
    {
        this.Animation.TimeElapsed += timeElapsed;
        this.Animation.Apply(this, timeElapsed);
    }

    // TODO @cultbaus: I want to use string formatting and text tags instead of this, but for now this can be a placeholder.
    private string Format(string originalValue)
    {
        var outMessage = originalValue;

        if (!this.Config.Message.Format)
        {
            return outMessage;
        }

        if (this.Config.Message.Prefix != string.Empty)
        {
            outMessage = $"{this.Config.Message.Prefix} {outMessage}";
        }

        if (this.Config.Message.Suffix != string.Empty)
        {
            outMessage = $"{outMessage} {this.Config.Message.Prefix}";
        }

        return outMessage;
    }
}