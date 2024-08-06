namespace CBT;

using System;
using System.Collections.Generic;
using CBT.FlyText;
using CBT.Types;
using ImGuiNET;

/// <summary>
/// Initializes a new isntance of the <see cref="PluginManager"/> class.
/// </summary>
public class PluginManager() : IDisposable
{
    private readonly List<FlyTextEvent> eventStream = [];

    /// <summary>
    /// Dispose of unhandled FlyTextEvents lingering in the stream.
    /// </summary>
    public void Dispose()
    {
        this.eventStream.Clear();
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

    /// <summary>
    /// Update a FlyTextEvent by passing time and removing events which have timed out.
    /// </summary>
    /// <param name="timeElapsed">Time that has passed since the last Draw frame.</param>
    public void Update(float timeElapsed)
    {
        this.eventStream.ForEach(e => e.Update(timeElapsed));
        this.eventStream.RemoveAll(e => e.IsExpired);
    }
}