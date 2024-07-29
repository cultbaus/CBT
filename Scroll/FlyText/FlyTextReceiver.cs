namespace Scroll.FlyText;

using System;
using System.Collections.Generic;
using System.Linq;

using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Hooking;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.Game.Character;

using S = Dalamud.Game.Gui.FlyText;

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
                var category = GetCategory(kind);
                if (IsCombatKind(category))
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
        Service.PluginLog.Debug($"FlyTextCreated event \"{kind}\" has been handled by Scroll.");

        handled = true;
    }
}

internal unsafe partial class FlyTextReceiver
{
    protected static FlyTextCategory GetCategory(FlyTextKind kind)
    {
        var attr = typeof(FlyTextKind)
            .GetMember(kind.ToString())[0]
            .GetCustomAttributes(typeof(FlyTextCategoryAttribute), false);

        return attr.Length > 0
            ? ((FlyTextCategoryAttribute)attr[0]).Category
            : throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
    }

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

    protected static List<FlyTextCategory> CombatKinds
        => Enum.GetValues(typeof(FlyTextCategory))
            .Cast<FlyTextCategory>()
            .Where(value => value.HasFlag(FlyTextCategory.Combat))
            .ToList();

    protected static bool IsCombatKind(FlyTextCategory category)
        => CombatKinds.Contains(category);
}