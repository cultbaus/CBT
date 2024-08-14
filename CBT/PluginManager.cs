namespace CBT;

using System;
using System.Collections.Generic;
using CBT.FlyText.Configuration;
using CBT.Interface;
using CBT.Types;
using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.Game.Character;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using ImGuiNET;

/// <summary>
/// PluginManager instance.
/// </summary>
public unsafe partial class PluginManager : IDisposable
{
    private readonly List<FlyTextEvent> eventStream = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="PluginManager"/> class.
    /// </summary>
    public PluginManager()
    {
        Service.Framework.Update += this.FrameworkUpdate;
    }

    /// <summary>
    /// Dispose of unhandled FlyTextEvents lingering in the stream.
    /// </summary>
    public void Dispose()
    {
        this.eventStream?.Clear();

        Service.Framework.Update -= this.FrameworkUpdate;

        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Add a FlyTextEvent to the event stream.
    /// </summary>
    /// <param name="flyTextEvent">A FlyText event.</param>
    public void Add(FlyTextEvent flyTextEvent)
    {
        this.eventStream?.Add(flyTextEvent);
    }

    /// <summary>
    /// Draw a FlyTextEvent to the Canvas.
    /// </summary>
    /// <param name="drawList">ImGui DrawList pointer.</param>
    public void Draw(ImDrawListPtr drawList)
    {
        FlyTextArtist.Draw(drawList, this.eventStream);
    }

    private void FrameworkUpdate(IFramework framework)
    {
        if (this.eventStream.Count == 0)
        {
            return;
        }

        var expiredEvents = new List<FlyTextEvent>();

        this.eventStream?.ForEach(e =>
        {
            e.Update(framework.UpdateDelta.Milliseconds);

            if (e.IsExpired)
            {
                expiredEvents.Add(e);
            }
        });
        expiredEvents.ForEach(e =>
        {
            this.eventStream?.Remove(e);

            Service.Pool.Put(e);
        });
    }
}

/// <summary>
/// FlyTextReceiver static member partial class implementation.
/// </summary>
public unsafe partial class PluginManager
{
    /// <summary>
    /// Gets the Local Player from the Dalamud client state.
    /// </summary>
    public static IPlayerCharacter? LocalPlayer
        => Service.ClientState.LocalPlayer;

    /// <summary>
    /// Get the config for the kind.
    /// </summary>
    /// <param name="kind">Kind to query config for.</param>
    /// <returns>The configuration options.</returns>
    public static FlyTextConfiguration? GetConfigForKind(FlyTextKind kind)
        => Service.Configuration.FlyTextKinds.TryGetValue(kind, out var config) ? config : null;

    /// <summary>
    /// Is the target an enemy.
    /// </summary>
    /// <param name="target">Target of the event.</param>
    /// <returns>A bool if they're a bad guy.</returns>
    public static bool IsEnemy(Character* target)
        => target->ObjectKind == ObjectKind.BattleNpc && target->SubKind == 5;

    /// <summary>
    /// Is the source a party member.
    /// </summary>
    /// <param name="source">Target of the event.</param>
    /// <returns>Are they in our party.</returns>
    public static bool IsPartyMember(Character* source)
        => source->IsPartyMember;

    /// <summary>
    /// Determines if a character instance is the player character.
    /// </summary>
    /// <param name="character">Character instance.</param>
    /// <returns>A boolean which is true if the Character is the player.</returns>
    public static bool IsPlayerCharacter(Character* character)
        => LocalPlayer?.GameObjectId == character->GetGameObjectId().ObjectId;

    /// <summary>
    /// Involves events for which the player is the source or the target.
    /// </summary>
    /// <param name="source">Source which an action originated from.</param>
    /// <param name="target">Target of the action.</param>
    /// <returns>True if the target or source is the player character.</returns>
    public static bool InvolvesMe(Character* source, Character* target)
        => IsPlayerCharacter(source) || IsPlayerCharacter(target);

    /// <summary>
    /// Determines if an event is unfiltered.
    /// </summary>
    /// <param name="kind">FlyTextKind of the event.</param>
    /// <returns>True if the event is enabled.</returns>
    public static bool Unfiltered(FlyTextKind kind)
        => Service.Configuration.FlyTextKinds[kind].Enabled;

    /// <summary>
    /// Gets the distance from the player to the target.
    /// </summary>
    /// <param name="target">Target of the FlyTextEvent.</param>
    /// <returns>The Yalm Distance from the target to the player.</returns>
    public static double GetDistance(Character* target)
    {
        if (target is null)
        {
            return 0;
        }

        var x = target->YalmDistanceFromPlayerX;
        var y = target->YalmDistanceFromPlayerZ;

        return Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
    }
}