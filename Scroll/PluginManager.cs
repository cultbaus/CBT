namespace Scroll;

using System;
using System.Collections.Generic;

using ImGuiNET;

using Scroll.FlyText;

internal class PluginManager : IDisposable
{
    private FlyTextArtist artist;
    private List<FlyTextEvent> eventStream = new List<FlyTextEvent>();

    internal PluginManager(FlyTextArtist artist)
    {
        this.artist = artist;
    }

    public void Dispose()
    {
        this.eventStream.Clear();
    }

    internal void Add(FlyTextEvent flyTextEvent)
        => this.eventStream.Add(flyTextEvent);

    internal void Update(float timeElapsed)
    {
        this.eventStream.ForEach(e => e.Update(timeElapsed));
        this.eventStream.RemoveAll(e => e.IsExpired);
    }

    public void Draw(ImDrawListPtr drawList)
        => this.artist.Draw(drawList, eventStream);
}