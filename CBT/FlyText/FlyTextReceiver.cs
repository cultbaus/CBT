namespace CBT.FlyText;

using System;
using System.Numerics;
using CBT.FlyText.Types;
using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Hooking;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.Game.Character;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using static FFXIVClientStructs.FFXIV.Client.Game.Character.ActionEffectHandler;
using DalamudFlyText = Dalamud.Game.Gui.FlyText;

/// <summary>
/// FlyTextReceiver receives FlyText from the game client.
/// </summary>
public unsafe partial class FlyTextReceiver : IDisposable
{
    private readonly Hook<AddScreenLogDelegate> addScreenLogHook;

    /// <summary>
    /// Initializes a new instance of the <see cref="FlyTextReceiver"/> class.
    /// </summary>
    /// <param name="gameInteropProvider">Dalamud game interop provider.</param>
    public FlyTextReceiver(IGameInteropProvider gameInteropProvider)
    {
        Service.FlyTextGui.FlyTextCreated += this.FlyTextCreated;

        this.addScreenLogHook = gameInteropProvider.HookFromAddress<AddScreenLogDelegate>(Service.Address.AddScreenLog, this.AddScreenLogDetour);
        this.addScreenLogHook.Enable();
    }

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

    private delegate void ReceiveActionEffectDelegate(
        uint casterEntityId,
        Character* casterPtr,
        Vector3* targetPos,
        Header* header,
        TargetEffects* effects,
        GameObjectId* targetEntityIds);

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
        try
        {
            var weActuallyCare = InvolvesMe(source, target) && Unfiltered(kind);

            if (weActuallyCare)
            {
                var effects = this.GetEffects(target->GetActionEffectHandler());
                var flyTextEvent = new FlyTextEvent(kind, effects, target, source, option, actionKind, actionID, val1, val2, val3, val4);

                Service.Manager.Add(flyTextEvent);

                var debugStr = string.Empty;

                debugStr += $"Handled: {kind} ";
                debugStr += $"for ability: {flyTextEvent.Name} ";

                if (flyTextEvent.DamageKind != null)
                {
                    debugStr += $"with DamageKind: {flyTextEvent.DamageKind} ";
                }

                debugStr += $"with iconID: {flyTextEvent.IconID}";

                Service.PluginLog.Info(debugStr);
            }
        }
        catch (Exception ex)
        {
            Service.PluginLog.Error($"CBT Wizard used Testicular Torsion: {ex.Message}");
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

    /// <summary>
    /// There's an pointer to the action effect handler on the target that allows me to get at the action effects.
    /// I can get the damage type from this, otherwise I need to share state between two delegates, and pointer arithmetic
    /// seemed like the lesser of two evils.
    /// </summary>
    /// <param name="handler">Action Effect Handler associated with the target.</param>
    /// <param name="effectEntryIndex">EffectEntry index. Maybe this changes someday.</param>
    /// <returns>A copy of of all effects that an action had.</returns>
    private Effect[] GetEffects(ActionEffectHandler* handler, int effectEntryIndex = 0)
    {
        var effectEntryPtr = (byte*)handler + (effectEntryIndex * 0x78);
        var targetEffectsPtr = (TargetEffects*)(effectEntryPtr + 0x38);
        var effectsSpan = targetEffectsPtr->Effects;

        return effectsSpan.ToArray();
    }
}

/// <summary>
/// FlyTextReceiver static member partial class implementation.
/// </summary>
public unsafe partial class FlyTextReceiver
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
        => Service.Configuration.FlyTextKinds[kind].Enabled;
}