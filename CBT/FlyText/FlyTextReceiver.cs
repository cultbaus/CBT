namespace CBT.FlyText;

using System;
using System.Linq;
using System.Numerics;
using CBT.Attributes;
using CBT.FlyText.Configuration;
using CBT.Interface.Tabs;
using CBT.Types;
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
    private readonly Hook<ReceiveActionEffectDelegate> receiveActionEffectHook;

    /// <summary>
    /// Initializes a new instance of the <see cref="FlyTextReceiver"/> class.
    /// </summary>
    /// <param name="gameInteropProvider">Dalamud game interop provider.</param>
    public FlyTextReceiver(IGameInteropProvider gameInteropProvider)
    {
        Service.FlyTextGui.FlyTextCreated += this.FlyTextCreated;

        this.addScreenLogHook = gameInteropProvider.HookFromAddress<AddScreenLogDelegate>(Service.Address.AddScreenLog, this.AddScreenLogDetour);
        this.addScreenLogHook.Enable();

        this.receiveActionEffectHook = gameInteropProvider.HookFromAddress<ReceiveActionEffectDelegate>(Service.Address.ReceiveActionEffect, this.ReceiveActionEffectDetour);
        this.receiveActionEffectHook.Enable();
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

        this.receiveActionEffectHook.Disable();
        this.receiveActionEffectHook.Dispose();

        Service.FlyTextGui.FlyTextCreated -= this.FlyTextCreated;

        GC.SuppressFinalize(this);
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
            var kindConfig = PluginManager.GetConfigForKind(kind);

            var effects = GetEffects(target->GetActionEffectHandler());
            var sourceObjectID = GetGameObjectId(target->GetActionEffectHandler());

            if (ShouldManageEvent(kind, source, target, sourceObjectID.ObjectId, kindConfig) && (kindConfig?.Enabled ?? false))
            {
                var flyTextEvent = Service.Pool.Get();
                flyTextEvent.Hydrate(kind, effects, sourceObjectID.ObjectId, target, source, option, actionKind, actionID, val1, val2, val3, val4);

                Service.Manager.Add(flyTextEvent);
            }
        }
        catch (Exception ex)
        {
            Service.PluginLog.Error($"CBT Wizard used Testicular Torsion: {ex.Message}");
        }

        this.addScreenLogHook.Original(target, source, kind, option, actionKind, actionID, val1, val2, val3, val4);
    }

    private void ReceiveActionEffectDetour(
        uint casterEntityId,
        Character* casterPtr,
        Vector3* targetPos,
        Header* header,
        TargetEffects* effects,
        GameObjectId* targetEntityIds)
    {
        this.receiveActionEffectHook.Original(casterEntityId, casterPtr, targetPos, header, effects, targetEntityIds);
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
        if (Service.Configuration.Options.TryGetValue(GlobalOption.NativeUnhandled.ToString(), out var allowNativeEvents))
        {
            var kindConfig = PluginManager.GetConfigForKind((FlyTextKind)kind);

            if (kindConfig == null)
            {
                return;
            }

            if (allowNativeEvents)
            {
                handled = kindConfig.Enabled;
            }
            else
            {
                handled = true;
            }
        }
    }
}

/// <summary>
/// Bad guy stuff.
/// </summary>
public unsafe partial class FlyTextReceiver
{
    /// <summary>
    /// There's a pointer to the action effect handler on the target that allows me to get at the action effects.
    /// I can get the damage type from this, otherwise I need to share state between two delegates, and pointer arithmetic
    /// seemed like the lesser of two evils.
    /// </summary>
    /// <param name="handler">Action Effect Handler associated with the target.</param>
    /// <param name="effectEntryIndex">EffectEntry index. Maybe this changes someday.</param>
    /// <returns>A copy of of all effects that an action had.</returns>
    private static Effect[] GetEffects(ActionEffectHandler* handler, int effectEntryIndex = 0)
    {
        var effectEntryPtr = (byte*)handler + (effectEntryIndex * 0x78);
        var targetEffectsPtr = (TargetEffects*)(effectEntryPtr + 0x38);
        var effectsSpan = targetEffectsPtr->Effects;

        return effectsSpan.ToArray();
    }

    private static GameObjectId GetGameObjectId(ActionEffectHandler* handler, int effectEntryIndex = 0)
    {
        var effectEntryPtr = (byte*)handler + (effectEntryIndex * 0x78);
        var gameObjectID = (GameObjectId*)(effectEntryPtr + 0x18);

        return *gameObjectID;
    }

    /// <summary>
    /// Determines if an event should be managed by the plugin or ignored.
    /// </summary>
    /// <param name="kind">Kind of the event. Unused.</param>
    /// <param name="source">Source of the event.</param>
    /// <param name="target">Target of the event.</param>
    /// <param name="sourceObjectID">Caster of the event.</param>
    /// <param name="kindConfig">Configuration for the current kind.</param>
    /// <returns>A bool indicating whether the plugin should manage the flytext event.</returns>
    private static bool ShouldManageEvent(FlyTextKind kind, Character* source, Character* target, uint sourceObjectID, FlyTextConfiguration kindConfig)
    {
        if (kindConfig == null)
        {
            return false;
        }

        if (!kindConfig.Enabled)
        {
            return false;
        }

        if (target == null || source == null)
        {
            return false;
        }

        if (PluginManager.GetDistance(target) > 60)
        {
            return false;
        }

        var isOurAbility = PluginManager.IsPlayerCharacter(source);
        var isAgainstUs = PluginManager.IsPlayerCharacter(target);

        var isPartyAbility = PluginManager.IsPartyMember(source);
        var isAgainstParty = PluginManager.IsPartyMember(target);

        var isEnemyAbility = PluginManager.IsEnemy(source);
        var isAgainstEnemy = PluginManager.IsEnemy(target);

        var specialCaseForHpRegen =
            kind == FlyTextKind.Healing
                && PluginManager.IsPartyMember(source)
                && PluginManager.LocalPlayer?.GameObjectId == sourceObjectID;

        var conditionTable = new (bool Condition, FlyTextFilter Filter)[]
        {
            (isOurAbility && isAgainstEnemy && kindConfig.Filter.Enemy, FlyTextFilter.Enemy),
            (isOurAbility && isAgainstUs && kindConfig.Filter.Self, FlyTextFilter.Self),
            (isOurAbility && isAgainstParty && kindConfig.Filter.Party, FlyTextFilter.Party),
            (isPartyAbility && isAgainstUs && kindConfig.Filter.Self, FlyTextFilter.Self),
            (isEnemyAbility && isAgainstUs && kindConfig.Filter.Self, FlyTextFilter.Self),
            (specialCaseForHpRegen && kindConfig.Filter.Party, FlyTextFilter.Party),
        };

        return conditionTable.Any(c => c.Condition && kind.ShouldAllow(c.Filter));
    }
}