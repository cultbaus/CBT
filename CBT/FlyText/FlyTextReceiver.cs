namespace CBT.FlyText;

using System;
using CBT.FlyText.Types;
using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Hooking;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.Game.Character;
using DalamudFlyText = Dalamud.Game.Gui.FlyText;

/// <summary>
/// FlyTextReceiver receives FlyText from the game client.
/// </summary>
internal unsafe partial class FlyTextReceiver : IDisposable
{
    private readonly Hook<AddScreenLogDelegate> addScreenLogHook;

    /// <summary>
    /// Initializes a new instance of the <see cref="FlyTextReceiver"/> class.
    /// </summary>
    /// <param name="gameInteropProvider">Dalamud game interop provider.</param>
    internal FlyTextReceiver(IGameInteropProvider gameInteropProvider)
    {
        Service.FlyTextGui.FlyTextCreated += this.FlyTextCreated;

        this.addScreenLogHook = gameInteropProvider.HookFromAddress<AddScreenLogDelegate>(Service.Address.AddScreenLog, this.AddScreenLogDetour);
        this.addScreenLogHook.Enable();
    }

    /// <summary>
    /// Delegate function for the AddScreenLog event.
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
    private delegate void AddScreenLogDelegate(
        Character* target,
        Character* source,
        FlyTextKind kind,
        int option,
        int actionKind,
        int actionID,
        int val1,
        int val2,
        int val3,
        int val4);

    /// <inheritdoc/>
    public void Dispose()
    {
        this.addScreenLogHook.Disable();
        this.addScreenLogHook.Dispose();

        Service.FlyTextGui.FlyTextCreated -= this.FlyTextCreated;
    }

    private void AddScreenLogDetour(
        Character* target,
        Character* source,
        FlyTextKind kind,
        int option,
        int actionKind,
        int actionID,
        int val1,
        int val2,
        int val3,
        int val4)
    {
        // TODO @cultbaus: Right now we only filter for combat events against the enemy, but eventually this should be
        // configurable such that a user may determine if they want to see buffs/debuffs, friendly healing, etc.
        try
        {
            if (InvolvesOther(source, target))
            {
                if (IsCombatKind(kind))
                {
                    Service.Manager.Add(new FlyTextEvent(kind, target, source, option, actionKind, actionID, val1, val2, val3, val4));
                }
            }

            // FIXME @cultbaus: Just a smoke test for healing
            else if (InvolvesMe(source, target))
            {
                if (IsHealing(kind))
                {
                    Service.Manager.Add(new FlyTextEvent(kind, target, source, option, actionKind, actionID, val1, val2, val3, val4));
                }
            }
        }
        catch (Exception ex)
        {
            Service.PluginLog.Error($"{ex.Message}");
        }

        this.addScreenLogHook.Original(target, source, kind, option, actionKind, actionID, val1, val2, val3, val4);
    }

    private void FlyTextCreated(
        ref DalamudFlyText.FlyTextKind kind,
        ref int val1,
        ref int val2,
        ref SeString text1,
        ref SeString text2,
        ref uint color,
        ref uint icon,
        ref uint damageTypeIcon,
        ref float yOffset,
        ref bool handled)
    {
        Service.PluginLog.Debug($"FlyTextCreated event \"{kind}\" has been handled by CBT.");

        handled = true;
    }
}

/// <summary>
/// FlyTextReceiver static member partial class implementation.
/// </summary>
internal unsafe partial class FlyTextReceiver
{
    /// <summary>
    /// Gets the Local Player from the Dalamud client state.
    /// </summary>
    protected static IPlayerCharacter? LocalPlayer
        => Service.ClientState.LocalPlayer;

    /// <summary>
    /// Determines if a character instance is the player character.
    /// </summary>
    /// <param name="character">Character instance.</param>
    /// <returns>A boolean which is true if the Character is the player.</returns>
    protected static bool IsPlayerCharacter(Character* character)
        => LocalPlayer?.GameObjectId == character->GetGameObjectId().ObjectId;

    /// <summary>
    /// Involves events for which the player is not the target but IS the source.
    /// </summary>
    /// <param name="source">Source which an action originated from.</param>
    /// <param name="target">Target of the action.</param>
    /// <returns>A bool.</returns>
    protected static bool InvolvesOther(Character* source, Character* target)
        => IsPlayerCharacter(source) && !IsPlayerCharacter(target);

    /// <summary>
    /// Involves events for which the player is the source or the target.
    /// </summary>
    /// <param name="source">Source which an action originated from.</param>
    /// <param name="target">Target of the action.</param>
    /// <returns>True if the target or source is the player character.</returns>
    protected static bool InvolvesMe(Character* source, Character* target)
        => IsPlayerCharacter(source) || IsPlayerCharacter(target);

    /// <summary>
    /// Determines if an event is unfiltered.
    /// </summary>
    /// <param name="kind">FlyTextKind of the event.</param>
    /// <returns>True if the event is enabled.</returns>
    protected static bool Unfiltered(FlyTextKind kind)
        => PluginConfiguration.FlyText[kind].Enabled;

    /// <summary>
    /// Checks if the event is of the Combat group.
    /// </summary>
    /// <param name="kind">FlyTextKind of the event.</param>
    /// <returns>True if the kind is in the Combat group.</returns>
    protected static bool IsCombatKind(FlyTextKind kind)
        => kind.InGroup(FlyTextCategory.Combat);

    /// <summary>
    /// Checks if the event is of the AbilityHealing category.
    /// </summary>
    /// <param name="kind">FlyTextKind of the event.</param>
    /// <returns>True if the kind is in the AbilityHealing category.</returns>
    protected static bool IsHealing(FlyTextKind kind)
        => kind.InCategory(FlyTextCategory.AbilityHealing);
}