namespace CBT.FlyText;

using System;

using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Hooking;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.Game.Character;

using S = Dalamud.Game.Gui.FlyText;

using CBT.FlyText.Types;

internal unsafe partial class FlyTextReceiver : IDisposable
{
    private readonly Hook<AddScreenLogDelegate> addScreenLogHook;

    internal FlyTextReceiver(IGameInteropProvider gameInteropProvider)
    {
        Service.FlyTextGui.FlyTextCreated += this.FlyTextCreated;

        this.addScreenLogHook = gameInteropProvider.HookFromAddress<AddScreenLogDelegate>(Service.Address.AddScreenLog, this.AddScreenLogDetour);
        this.addScreenLogHook.Enable();
    }

    public void Dispose()
    {
        this.addScreenLogHook.Disable();
        this.addScreenLogHook.Dispose();

        Service.FlyTextGui.FlyTextCreated -= this.FlyTextCreated;
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
            if (InvolvesEnemy(source, target))
            {
                if (IsCombatKind(kind))
                    Service.Manager.Add(new FlyTextEvent(kind, target, source, option, actionKind, actionID, val1, val2, val3, val4));
            }

            // FIXME @cultbaus: Just a smoke test for healing
            else if (InvolvesMe(source, target))
            {
                if (IsHealing(kind))
                    Service.Manager.Add(new FlyTextEvent(kind, target, source, option, actionKind, actionID, val1, val2, val3, val4));
            }
        }
        catch (Exception ex)
        {
            Service.PluginLog.Error($"{ex.Message}");
        }

        this.addScreenLogHook.Original(target, source, kind, option, actionKind, actionID, val1, val2, val3, val4);
    }

    private void FlyTextCreated(
        ref S.FlyTextKind kind,
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

internal unsafe partial class FlyTextReceiver
{
    protected static IPlayerCharacter? LocalPlayer
        => Service.ClientState.LocalPlayer;

    protected static bool IsPlayerCharacter(Character* character)
        => LocalPlayer?.GameObjectId == character->GetGameObjectId().ObjectId;

    protected static bool InvolvesEnemy(Character* source, Character* target)
        => IsPlayerCharacter(source) && !IsPlayerCharacter(target);

    protected static bool InvolvesMe(Character* source, Character* target)
        => IsPlayerCharacter(source) || IsPlayerCharacter(target);

    protected static bool Unfiltered(FlyTextKind kind)
        => Service.Configuration.FlyText[kind].Enabled;

    protected static bool IsCombatKind(FlyTextKind kind)
        => kind.InGroup(FlyTextCategory.Combat);

    protected static bool IsHealing(FlyTextKind kind)
        => kind.InCategory(FlyTextCategory.AbilityHealing);
}