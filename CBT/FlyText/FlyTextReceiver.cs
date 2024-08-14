namespace CBT.FlyText;

using System;
using CBT.Attributes;
using CBT.Types;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Hooking;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.Game.Character;
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

    /// <inheritdoc/>
    public void Dispose()
    {
        this.addScreenLogHook.Disable();
        this.addScreenLogHook.Dispose();

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
            var weActuallyCare = false;

            // I am leaving this exactly the way it is and you can't stop me.
            if (kindConfig?.Enabled ?? false)
            {
                var isOurAbility = PluginManager.IsPlayerCharacter(source);
                var isAgainstUs = PluginManager.IsPlayerCharacter(target);

                var isAllyAbility = PluginManager.IsPartyMember(source);
                var isAgainstAlly = PluginManager.IsPartyMember(target);

                var isEnemyAbility = PluginManager.IsEnemy(source);
                var isAgainstEnemy = PluginManager.IsEnemy(target);

                if (isOurAbility && isAgainstEnemy && kind.ShouldAllow(FlyTextFilter.Enemy))
                {
                    weActuallyCare = true;
                }
                else if (isOurAbility && isAgainstUs && kind.ShouldAllow(FlyTextFilter.Self))
                {
                    weActuallyCare = true;
                }
                else if (isOurAbility && isAgainstAlly && kind.ShouldAllow(FlyTextFilter.Party))
                {
                    weActuallyCare = true;
                }
                else if (isAllyAbility && isAgainstUs && kind.ShouldAllow(FlyTextFilter.Self))
                {
                    weActuallyCare = true;
                }
                else if (isEnemyAbility && isAgainstUs && kind.ShouldAllow(FlyTextFilter.Self))
                {
                    weActuallyCare = true;
                }

                if (weActuallyCare)
                {
                    var effects = GetEffects(target->GetActionEffectHandler());

                    var flyTextEvent = Service.Pool.Get();
                    flyTextEvent.Hydrate(kind, effects, target, source, option, actionKind, actionID, val1, val2, val3, val4);

                    Service.Manager.Add(flyTextEvent);
                }
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
}