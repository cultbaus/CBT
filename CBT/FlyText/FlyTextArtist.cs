namespace CBT.FlyText;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using CBT.Helpers;
using CBT.Types;
using Dalamud.Interface.Utility.Raii;
using ImGuiNET;

/// <summary>
/// FlyTextArtist draws FlyText to the main CBT Canvas.
/// </summary>
public unsafe class FlyTextArtist
{

    /// <summary>
    /// Draws events to the CBT canvas.
    /// </summary>
    /// <param name="drawList">ImGUI Draw List.</param>
    /// <param name="flyTextEvents">Events to draw to the canvas.</param>
    public static void Draw(ImDrawListPtr drawList, List<FlyTextEvent> flyTextEvents)
    {
        QuadTreeManager.Clear();

        flyTextEvents.ForEach(e =>
        {
            QuadTree qt = QuadTreeManager.GetQuadTree(e.Target->GetGameObjectId().ObjectId);
            qt.Insert(e);
        });
        flyTextEvents.ForEach(e =>
        {
            QuadTree qt = QuadTreeManager.GetQuadTree(e.Target->GetGameObjectId().ObjectId);

            var potentialCollisions = qt.Retrieve([], e);
            potentialCollisions.ForEach(p =>
            {
                if (p != e)
                {
                    AdjustOverlap(e, p);
                }
            });
        });
        flyTextEvents.ForEach(e => DrawFlyTextWithIconAndOutlines(drawList, e));
    }

    private static Vector2 Center(FlyTextEvent flyTextEvent)
    {
        return new(flyTextEvent.Position.X - (flyTextEvent.Size.X / 2), flyTextEvent.Position.Y - (flyTextEvent.Size.Y / 2));
    }

    private static bool IsOverlapping(FlyTextEvent a, FlyTextEvent b)
    {
        var aRect = new Rectangle(a.Position.X, a.Position.Y, a.Size.X, a.Size.Y);
        var bRect = new Rectangle(b.Position.X, b.Position.Y, b.Size.X, b.Size.Y);

        return aRect.Intersects(bRect);
    }

    private static float GetOverlap(FlyTextEvent a, FlyTextEvent b)
    {
        var aBottom = a.Position.Y + a.Size.Y;
        var bBottom = b.Position.Y + b.Size.Y;

        return Math.Max(0, Math.Min(aBottom, bBottom) - Math.Max(a.Position.Y, b.Position.Y));
    }

    private static void AdjustOverlap(FlyTextEvent a, FlyTextEvent b)
    {
        if (IsOverlapping(a, b))
        {
            var toAdjust = a.Animation.TimeElapsed < b.Animation.TimeElapsed ? a : b;
            toAdjust.Animation.Offset = new Vector2(toAdjust.Animation.Offset.X, toAdjust.Animation.Offset.Y + GetOverlap(a, b));
        }
    }

    private static void DrawFlyTextWithIconAndOutlines(ImDrawListPtr drawList, FlyTextEvent flyTextEvent)
    {
        using (ImRaii.PushStyle(ImGuiStyleVar.Alpha, flyTextEvent.Animation.Alpha))
        {
            using (Service.Fonts.Push(flyTextEvent.Config.Font.Name, flyTextEvent.Config.Font.Size))
            {
                if (flyTextEvent.Config.Icon.Enabled)
                {
                    DrawIcon(drawList, flyTextEvent);
                }

                DrawText(drawList, flyTextEvent);
            }
        }
    }

    private static void DrawText(ImDrawListPtr drawList, FlyTextEvent flyTextEvent)
    {
        if (flyTextEvent.Config.Font.Outline.Enabled)
        {
            DrawTextOutline(drawList, flyTextEvent);
        }

        drawList.AddText(Center(flyTextEvent), ImGui.GetColorU32(flyTextEvent.Config.Font.Color), flyTextEvent.Text);
    }

    private static void DrawIcon(ImDrawListPtr drawList, FlyTextEvent flyTextEvent)
    {
        if (flyTextEvent.Icon == null)
        {
            return;
        }

        var textPos = Center(flyTextEvent);
        var iconSize = flyTextEvent.Config.Icon.Size;
        var iconAlpha = (uint)(flyTextEvent.Animation.Alpha * 255.0f) << 24 | 0x00FFFFFF;
        var verticalOffset = (ImGui.CalcTextSize(flyTextEvent.Text).Y - iconSize.Y) / 2.0f;
        var iconPos = new Vector2(textPos.X - iconSize.X + flyTextEvent.Config.Icon.Offset.X, textPos.Y + verticalOffset + flyTextEvent.Config.Icon.Offset.Y);
        var uvOffset = (1.0f - flyTextEvent.Config.Icon.Zoom) / 2.0f;
        var uvMin = new Vector2(uvOffset, uvOffset);
        var uvMax = new Vector2(1.0f - uvOffset, 1.0f - uvOffset);

        if (flyTextEvent.Config.Icon.Outline.Enabled)
        {
            DrawIconOutline(drawList, flyTextEvent, iconPos, iconSize);
        }

        drawList.AddImage(flyTextEvent.Icon.ImGuiHandle, iconPos, iconPos + iconSize, uvMin, uvMax, iconAlpha);
    }

    private static void DrawIconOutline(ImDrawListPtr drawList, FlyTextEvent flyTextEvent, Vector2 iconPos, Vector2 iconSize)
    {
        var borderSize = flyTextEvent.Config.Icon.Outline.Size;
        var borderColor = ImGui.GetColorU32(flyTextEvent.Config.Icon.Outline.Color);
        var borderMin = new Vector2(iconPos.X - borderSize, iconPos.Y - borderSize);
        var borderMax = new Vector2(iconPos.X + iconSize.X + borderSize, iconPos.Y + iconSize.Y + borderSize);

        drawList.AddRect(borderMin, borderMax, borderColor, 0.0f, ImDrawFlags.None, borderSize);
    }

    private static void DrawTextOutline(ImDrawListPtr drawList, FlyTextEvent flyTextEvent)
    {
        var textPosition = Center(flyTextEvent);
        var outlineColor = ImGui.GetColorU32(flyTextEvent.Config.Font.Outline.Color);

        var offsets = Enumerable.Range(1, flyTextEvent.Config.Font.Outline.Size)
            .SelectMany(i => new[]
            {
                new Vector2(-i, i),
                new Vector2(0, i),
                new Vector2(i, i),
                new Vector2(-i, 0),
                new Vector2(i, 0),
                new Vector2(-i, -i),
                new Vector2(0, -i),
                new Vector2(i, -i),
            });

        foreach (var offset in offsets)
        {
            drawList.AddText(textPosition + offset, outlineColor, flyTextEvent.Text);
        }
    }
}