namespace CBT.Types;

using System;
using System.Data;
using System.Linq;
using System.Numerics;
using CBT.FlyText.Animations;
using CBT.FlyText.Configuration;
using CBT.Helpers;
using Dalamud.Interface.Textures;
using Dalamud.Interface.Textures.TextureWraps;
using FFXIVClientStructs.FFXIV.Client.Game.Character;
using ImGuiNET;
using static FFXIVClientStructs.FFXIV.Client.Game.Character.ActionEffectHandler;

/// <summary>
/// The fly text event.
/// </summary>
public unsafe partial class FlyTextEvent
{
    private Vector2? flyTextSize;

    /// <summary>
    /// Initializes a new instance of the <see cref="FlyTextEvent"/> class.
    /// </summary>
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
    public FlyTextEvent(FlyTextKind kind, Effect[] effects, Character* target, Character* source, int option, int actionKind, int actionID, int val1, int val2, int val3, int val4)
    {
        this.Kind = kind;
        this.Effects = effects;
        this.Target = target;
        this.Source = source;
        this.Option = option;
        this.ActionKind = actionKind;
        this.ActionID = actionID;
        this.Value1 = val1;
        this.Value2 = val2;
        this.Value3 = val3;
        this.Value4 = val4;

        this.Config = new FlyTextConfiguration(kind);
        this.Animation = FlyTextAnimation.Create(kind);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FlyTextEvent"/> class.
    /// </summary>
    public FlyTextEvent()
    {
        this.Effects = null!;
        this.Target = null!;
        this.Source = null!;
        this.Option = default;
        this.ActionKind = default;
        this.ActionID = default;
        this.Value1 = default;
        this.Value2 = default;
        this.Value3 = default;
        this.Value4 = default;

        this.Config = null!;
        this.Animation = null!;
    }

    /// <summary>
    /// gets a value indicating whether an event is expired.
    /// </summary>
    public bool IsExpired
        => this.Animation?.TimeElapsed > this.Animation?.Duration;

    /// <summary>
    /// Gets a value indicating the world anchor of the event.
    /// </summary>
    public Vector2 Anchor
    {
        get
        {
            if (PluginManager.IsPlayerCharacter(this.Target))
            {
                Service.Configuration.Options.TryGetValue(GlobalOption.PlayerAnchor.ToString(), out var isAnchorFree);

                if (isAnchorFree)
                {
                    return Service.Configuration.FreeMoveAnchor;
                }
            }

            return Service.GameGui.WorldToScreen(this.Target->Position, out Vector2 currentPosition) ? currentPosition : Vector2.Zero;
        }
    }

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
        get
        {
            if (this.flyTextSize != null)
            {
                return (Vector2)this.flyTextSize;
            }
            else
            {
                using (Service.Fonts.Push(this.Config.Font.Name, this.Config.Font.Size))
                {
                    var textSize = ImGui.CalcTextSize(this.Text);

                    if (!this.Config.Icon.Enabled)
                    {
                        return textSize;
                    }

                    var iconSize = this.Config.Icon.Enabled ? this.Config.Icon.Size : Vector2.Zero;

                    var totalWidth = iconSize.X + textSize.X + this.Config.Icon.Offset.X;
                    var totalHeight = Math.Max(textSize.Y, iconSize.Y);

                    this.flyTextSize = new Vector2(totalWidth, totalHeight);
                }

                return (Vector2)this.flyTextSize;
            }
        }
    }

    /// <summary>
    /// Gets the string representation of the FlyTextEvent Value1.
    /// </summary>
    public string Text
        => this.Kind.IsStatus() ? this.Format(this.Name) : this.Kind.IsMessage() ? this.Format(this.Kind.Pretty()) : this.Format(this.Value1.ToString("N0"));

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
    /// Gets the positional state for an action.
    /// </summary>
    public PositionalState PositionalState
        => this.Kind.InCategory(FlyTextCategory.AbilityDamage)
            ? PositionalManager
                .PositionalSucceeded(this.ActionID, this.Effects.ToList().Where(effect => (ActionEventKind)effect.Type == ActionEventKind.Damage).FirstOrDefault().Param2)
            : PositionalState.None;

    /// <summary>
    /// Gets the color for a flytext event.
    /// </summary>
    public Vector4 Color
    {
        get
        {
            if (this.Config.Positionals)
            {
                return this.PositionalState switch
                {
                    PositionalState.None => this.Config.Font.Color,
                    PositionalState.Success => this.Config.Font.ColorSuccess,
                    PositionalState.Failed => this.Config.Font.ColorFailed,
                    _ => throw new ArgumentOutOfRangeException(nameof(this.PositionalState), this.PositionalState, null),
                };
            }
            else
            {
                return this.Config.Font.Color;
            }
        }
    }

    /// <summary>
    /// Gets or sets the fly text kind.
    /// </summary>
    public FlyTextKind Kind { get; set; }

    /// <summary>
    /// Gets or sets the FlyTextConfiguration.
    /// </summary>
    public FlyTextConfiguration Config { get; set; }

    /// <summary>
    /// Gets or sets the FlyTextAnimation.
    /// </summary>
    public FlyTextAnimation Animation { get; set; }

    /// <summary>
    /// Gets or sets the Effects associated with the event.
    /// </summary>
    public Effect[]? Effects { get; set; }

    /// <summary>
    /// Gets or sets the Target from the original event.
    /// </summary>
    public Character* Target { get; set; }

    /// <summary>
    /// Gets or sets the Source from the original event.
    /// </summary>
    public Character* Source { get; set; }

    /// <summary>
    /// Gets or sets the Option from the original event.
    /// </summary>
    public int Option { get; set; }

    /// <summary>
    /// Gets or sets the ActionKind from the original event.
    /// </summary>
    public int ActionKind { get; set; }

    /// <summary>
    /// Gets or sets the ActionID from the original event.
    /// </summary>
    public int ActionID { get; set; }

    /// <summary>
    /// Gets or sets the Value1 from the original event.
    /// </summary>
    public int Value1 { get; set; }

    /// <summary>
    /// Gets or sets the Value2 from the original event.
    /// </summary>
    public int Value2 { get; set; }

    /// <summary>
    /// Gets or sets the Value3 from the original event.
    /// </summary>
    public int Value3 { get; set; }

    /// <summary>
    /// Gets or sets the Value4 from the original event.
    /// </summary>
    public int Value4 { get; set; }

    /// <summary>
    /// Update applies state changes to the FlyText event.
    /// </summary>
    /// <param name="timeElapsed">Time since the last frame.</param>
    public void Update(float timeElapsed)
    {
        if (this.Animation != null)
        {
            this.Animation.TimeElapsed += timeElapsed / 1000;
            this.Animation.Apply(this, timeElapsed / 1000);
        }
    }

    /// <summary>
    /// Reset the state.
    /// </summary>
    public void Reset()
    {
        this.Kind = FlyTextKind.None;
        this.Effects = null!;
        this.Target = null!;
        this.Source = null!;
        this.Option = default;
        this.ActionKind = default;
        this.ActionID = default;
        this.Value1 = default;
        this.Value2 = default;
        this.Value3 = default;
        this.Value4 = default;

        this.Config = null!;
        this.Animation = null!;
    }

    /// <summary>
    /// Give life.
    /// </summary>
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
    public void Hydrate(FlyTextKind kind, Effect[] effects, Character* target, Character* source, int option, int actionKind, int actionID, int val1, int val2, int val3, int val4)
    {
        this.Kind = kind;
        this.Effects = effects;
        this.Target = target;
        this.Source = source;
        this.Option = option;
        this.ActionKind = actionKind;
        this.ActionID = actionID;
        this.Value1 = val1;
        this.Value2 = val2;
        this.Value3 = val3;
        this.Value4 = val4;

        this.Config = new FlyTextConfiguration(kind);
        this.Animation = FlyTextAnimation.Create(kind);
    }

    // TODO @cultbaus: I want to use string formatting and text tags instead of this, but for now this can be a placeholder.
    private string Format(string originalValue)
    {
        var outMessage = originalValue;

        if (this.Config != null)
        {
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
                ;
                outMessage = $"{outMessage} {this.Config.Message.Prefix}";
            }

            return outMessage;
        }

        return originalValue;
    }
}