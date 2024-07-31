namespace CBT.FlyText;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

using ImGuiNET;

using CBT.FlyText.Types;
using System.Security.Cryptography.X509Certificates;
using System.Formats.Tar;

internal unsafe partial class FlyTextArtist
{
    internal void Draw(ImDrawListPtr drawList, List<FlyTextEvent> flyTextEvents)
    {
        var indexedEvents = flyTextEvents
            .Select((e, i) => new { Event = e, Index = i })
            .ToList();

        indexedEvents
            .ForEach(a =>
            {
                indexedEvents.Skip(a.Index + 1)
                    .ToList()
                    .ForEach(b => AdjustOverlap(a.Event, b.Event));

                this.DrawFlyText(drawList, a.Event);
            });
    }

    private void DrawFlyText(ImDrawListPtr drawList, FlyTextEvent flyTextEvent)
    {
        ImGui.PushStyleVar(ImGuiStyleVar.Alpha, flyTextEvent.Animation.Alpha);
        {
            using (Service.Fonts.Push(flyTextEvent.Config.Font.Name, flyTextEvent.Config.Font.Size))
            {
                if (flyTextEvent.Config.Outline.Enabled)
                    DrawOutline(drawList, flyTextEvent);

                // FIXME @cultbaus: Icons are incorrect, see FlyTextEvent.cs
                // DrawIcon(drawList, flyTextEvent);
                DrawText(drawList, flyTextEvent);
            }
        }
        ImGui.PopStyleVar();
    }
}

internal unsafe partial class FlyTextArtist
{
    protected static Vector2 VerticalCenter(FlyTextEvent flyTextEvent)
        => new Vector2(flyTextEvent.Position.X - (flyTextEvent.Size.X / 2), flyTextEvent.Position.Y - (flyTextEvent.Size.Y / 2));

    protected static float VerticalDelta(FlyTextEvent a, FlyTextEvent b)
        => Math.Abs(a.Position.Y - b.Position.Y);

    protected static bool IsOverlapping(FlyTextEvent a, FlyTextEvent b)
        => !(a.Position.X + a.Size.X < b.Position.X
                || a.Position.X > b.Position.X + b.Size.X
                || a.Position.Y + a.Size.Y < b.Position.Y
                || a.Position.Y > b.Position.Y + b.Size.Y);

    protected static void AdjustOverlap(FlyTextEvent a, FlyTextEvent b)
    {
        if (IsOverlapping(a, b))
            a.Animation.Offset = a.Animation.Offset with { Y = a.Animation.Offset.Y + (b.Size.Y - VerticalDelta(a, b)) };
    }

    protected static void DrawText(ImDrawListPtr drawList, FlyTextEvent flyTextEvent)
    {
        drawList.AddText(VerticalCenter(flyTextEvent), ImGui.GetColorU32(flyTextEvent.Config.Font.Color), flyTextEvent.Text);
    }

    protected static void DrawIcon(ImDrawListPtr drawList, FlyTextEvent flyTextEvent)
    {
        if (flyTextEvent.Icon == null)
            return;

        var pos = VerticalCenter(flyTextEvent);
        var iconSize = flyTextEvent.Icon.Size;

        var textHeight = flyTextEvent.Config.Font.Size;
        var aspectRatio = iconSize.X / iconSize.Y;
        var iconHeight = textHeight;
        var iconWidth = iconHeight * aspectRatio;

        var iconPos = new Vector2(pos.X - iconWidth - 4, pos.Y);

        drawList.AddImage(flyTextEvent.Icon.ImGuiHandle, iconPos, iconPos + new Vector2(iconWidth, iconHeight));
    }


    protected static void DrawOutline(ImDrawListPtr drawList, FlyTextEvent flyTextEvent)
    {
        var textPosition = VerticalCenter(flyTextEvent);
        var outlineColor = ImGui.GetColorU32(flyTextEvent.Config.Outline.Color);

        Enumerable.Range(1, flyTextEvent.Config.Outline.Size)
            .SelectMany(i => new[]{
                    new Vector2(-i, i),
                    new Vector2(0, i),
                    new Vector2(i, i),
                    new Vector2(-i, 0),
                    new Vector2(i, 0),
                    new Vector2(-i, -i),
                    new Vector2(0, -i),
                    new Vector2(i, -i),
            })
            .ToList()
            .ForEach(offset => drawList.AddText(textPosition + offset, outlineColor, flyTextEvent.Text));
    }
}