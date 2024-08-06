namespace CBT;

using System;
using System.Collections.Generic;
using CBT.FlyText;
using CBT.Types;
using Dalamud.Plugin.Services;
using ImGuiNET;

/// <summary>
/// PluginManager instance.
/// </summary>
public class PluginManager : IDisposable
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
        Service.Framework.Update -= this.FrameworkUpdate;

        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Add a FlyTextEvent to the event stream.
    /// </summary>
    /// <param name="flyTextEvent">A FlyText event.</param>
    public void Add(FlyTextEvent flyTextEvent)
    {
        this.eventStream.Add(flyTextEvent);
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
        this.eventStream.ForEach(e => e.Update(framework.UpdateDelta.Milliseconds));
        this.eventStream.RemoveAll(e => e.IsExpired);
    }
}