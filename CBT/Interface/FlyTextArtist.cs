namespace CBT.Interface;

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
        Service.Tree.Clear();

        flyTextEvents.ForEach(e =>
        {
            var qt = Service.Tree.GetQuadTree(e.Target->GetGameObjectId().ObjectId);

            qt.Insert(e);
            qt.Retrieve([], e)
                .ForEach(p =>
                {
                    if (p != e)
                    {
                        AdjustOverlap(e, p);
                    }
                });

            DrawFlyTextWithIconAndOutlines(drawList, e);
        });
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
        if (!flyTextEvent.Config.Font.Outline.Enabled)
        {
            return;
        }

        DrawTextOutline(drawList, flyTextEvent);

        drawList.AddText(Center(flyTextEvent), ImGui.GetColorU32(flyTextEvent.Color), flyTextEvent.Text);
    }

    private static void DrawTextOutline(ImDrawListPtr drawList, FlyTextEvent flyTextEvent)
    {
        static Vector2[] MakeVectors(int i)
        {
            return
            [
                new Vector2(-i, i),
                new Vector2(0, i),
                new Vector2(i, i),
                new Vector2(-i, 0),
                new Vector2(i, 0),
                new Vector2(-i, -i),
                new Vector2(0, -i),
                new Vector2(i, -i),
            ];
        }

        var textPosition = Center(flyTextEvent);
        var outlineColor = ImGui.GetColorU32(flyTextEvent.Config.Font.Outline.Color);

        Enumerable
            .Range(1, flyTextEvent.Config.Font.Outline.Size)
            .SelectMany(MakeVectors)
            .ToList()
            .ForEach(offset => drawList.AddText(textPosition + offset, outlineColor, flyTextEvent.Text));
    }

    private static void DrawIcon(ImDrawListPtr drawList, FlyTextEvent flyTextEvent)
    {
        if (flyTextEvent.Icon == null)
        {
            return;
        }

        var textPos = Center(flyTextEvent);
        var iconConfig = flyTextEvent.Config.Icon;
        var iconSize = iconConfig.Size;
        var iconAlpha = (uint)(flyTextEvent.Animation.Alpha * 255.0f) << 24 | 0x00FFFFFF;

        var iconPos = CalculateIconPosition(flyTextEvent, textPos, iconSize);

        if (iconConfig.Outline.Enabled)
        {
            DrawIconOutline(drawList, flyTextEvent, iconPos, iconSize);
        }

        var (uvMin, uvMax) = CalculateUvMinMax(iconConfig.Zoom);

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
        if (!IsOverlapping(a, b))
        {
            return;
        }

        if (a.Animation.AnimationKind != b.Animation.AnimationKind)
        {
            return;
        }

        if (a.Animation.Reversed != b.Animation.Reversed)
        {
            return;
        }

        (a.Animation.TimeElapsed < b.Animation.TimeElapsed ? a : b).Animation.Offset += new Vector2(0, GetOverlap(a, b));
    }

    private static Vector2 CalculateIconPosition(FlyTextEvent flyTextEvent, Vector2 textPos, Vector2 iconSize)
    {
        var verticalOffset = (ImGui.CalcTextSize(flyTextEvent.Text).Y - iconSize.Y) / 2.0f;
        return new Vector2(textPos.X - iconSize.X + flyTextEvent.Config.Icon.Offset.X, textPos.Y + verticalOffset + flyTextEvent.Config.Icon.Offset.Y);
    }

    private static (Vector2 Min, Vector2 Max) CalculateUvMinMax(float zoom)
    {
        var uvOffset = (1.0f - zoom) / 2.0f;
        return (Min: new Vector2(uvOffset, uvOffset), Max: new Vector2(1.0f - uvOffset, 1.0f - uvOffset));
    }
}