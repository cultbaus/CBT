namespace Scroll;

using System;
using System.Collections.Generic;

using ImGuiNET;

using Scroll.FlyText;
using Scroll.FlyText.Types;

internal class PluginManager(FlyTextArtist artist) : IDisposable
{
    private FlyTextArtist flyTextArtist = artist;
    private List<FlyTextEvent> eventStream = new List<FlyTextEvent>();

    public void Dispose()
        => this.eventStream.Clear();

    internal void Add(FlyTextEvent flyTextEvent)
        => this.eventStream.Add(flyTextEvent);

    internal void Draw(ImDrawListPtr drawList)
        => this.flyTextArtist.Draw(drawList, eventStream);

    internal void Update(float timeElapsed)
    {
        this.eventStream.ForEach(e => e.Update(timeElapsed));
        this.eventStream.RemoveAll(e => e.IsExpired);
    }
}