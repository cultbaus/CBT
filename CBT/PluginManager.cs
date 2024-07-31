namespace CBT;

using System;
using System.Collections.Generic;
using CBT.FlyText;
using CBT.FlyText.Types;
using ImGuiNET;

/// <summary>
/// Initializes a new isntance of the <see cref="PluginManager"/> class.
/// </summary>
/// <param name="artist">FlyTextArtist is an API which draws to the canvas.</param>
internal class PluginManager(FlyTextArtist artist) : IDisposable
{
    private readonly FlyTextArtist flyTextArtist = artist;

    private readonly List<FlyTextEvent> eventStream = new List<FlyTextEvent>();

    /// <summary>
    /// Dispose of unhandled FlyTextEvents lingering in the stream.
    /// </summary>
    public void Dispose()
        => this.eventStream.Clear();

    /// <summary>
    /// Add a FlyTextEvent to the event stream.
    /// </summary>
    /// <param name="flyTextEvent">A FlyText event.</param>
    internal void Add(FlyTextEvent flyTextEvent)
        => this.eventStream.Add(flyTextEvent);

    /// <summary>
    /// Draw a FlyTextEvent to the Canvas.
    /// </summary>
    /// <param name="drawList">ImGui DrawList pointer.</param>
    internal void Draw(ImDrawListPtr drawList)
        => FlyTextArtist.Draw(drawList, this.eventStream);

    /// <summary>
    /// Update a FlyTextEvent by passing time and removing events which have timed out.
    /// </summary>
    /// <param name="timeElapsed">Time that has passed since the last Draw frame.</param>
    internal void Update(float timeElapsed)
    {
        this.eventStream.ForEach(e => e.Update(timeElapsed));
        this.eventStream.RemoveAll(e => e.IsExpired);
    }
}