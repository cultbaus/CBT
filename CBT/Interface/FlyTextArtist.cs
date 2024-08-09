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
        if (flyTextEvents == null || flyTextEvents.Count == 0)
        {
            return;
        }

        Service.Tree.Clear();

        flyTextEvents.ForEach(e =>
        {
            if (e.Kind != FlyTextKind.None)
            {
                var qt = Service.Tree.GetQuadTree(e.Target->GetGameObjectId().ObjectId);

                qt.Insert(e);
                qt.Retrieve([], e)
                    .ForEach(p =>
                    {
                        if (p != e)
                        {
                            AdjustOverlap(ref e, ref p);
                        }
                    });

                DrawFlyTextWithIconAndOutlines(drawList, ref e);
            }
        });
    }

    private static void DrawFlyTextWithIconAndOutlines(ImDrawListPtr drawList, ref FlyTextEvent flyTextEvent)
    {
        using (ImRaii.PushStyle(ImGuiStyleVar.Alpha, flyTextEvent.Animation.Alpha))
        {
            using (Service.Fonts.Push(flyTextEvent.Config.Font.Name, flyTextEvent.Config.Font.Size))
            {
                if (flyTextEvent.Config.Icon.Enabled)
                {
                    DrawIcon(drawList, ref flyTextEvent);
                }

                DrawText(drawList, ref flyTextEvent);
            }
        }
    }

    private static void DrawText(ImDrawListPtr drawList, ref FlyTextEvent flyTextEvent)
    {
        if (!flyTextEvent.Config.Font.Outline.Enabled)
        {
            return;
        }

        DrawTextOutline(drawList, ref flyTextEvent);

        drawList.AddText(Center(flyTextEvent), ImGui.GetColorU32(flyTextEvent.Color), flyTextEvent.Text);
    }

    private static void DrawTextOutline(ImDrawListPtr drawList, ref FlyTextEvent flyTextEvent)
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
        var flyTextMessage = flyTextEvent.Text;

        Enumerable
            .Range(1, flyTextEvent.Config.Font.Outline.Size)
            .SelectMany(MakeVectors)
            .ToList()
            .ForEach(offset => drawList.AddText(textPosition + offset, outlineColor, flyTextMessage));
    }

    private static void DrawIcon(ImDrawListPtr drawList, ref FlyTextEvent flyTextEvent)
    {
        if (flyTextEvent.Icon == null)
        {
            return;
        }

        var textPos = Center(flyTextEvent);
        var iconConfig = flyTextEvent.Config.Icon;
        var iconSize = iconConfig.Size;
        var iconAlpha = (uint)(flyTextEvent.Animation.Alpha * 255.0f) << 24 | 0x00FFFFFF;

        var iconPos = CalculateIconPosition(ref flyTextEvent, ref textPos, ref iconSize);

        if (iconConfig.Outline.Enabled)
        {
            DrawIconOutline(drawList, ref flyTextEvent, ref iconPos, ref iconSize);
        }

        var (uvMin, uvMax) = CalculateUvMinMax(iconConfig.Zoom);

        drawList.AddImage(flyTextEvent.Icon?.ImGuiHandle ?? 0, iconPos, iconPos + iconSize, uvMin, uvMax, iconAlpha);
    }

    private static void DrawIconOutline(ImDrawListPtr drawList, ref FlyTextEvent flyTextEvent, ref Vector2 iconPos, ref Vector2 iconSize)
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

    private static bool IsOverlapping(ref FlyTextEvent a, ref FlyTextEvent b)
    {
        var aRect = new Rectangle(a.Position.X, a.Position.Y, a.Size.X, a.Size.Y);
        var bRect = new Rectangle(b.Position.X, b.Position.Y, b.Size.X, b.Size.Y);

        return aRect.Intersects(bRect);
    }

    private static float GetOverlap(ref FlyTextEvent a, ref FlyTextEvent b)
    {
        var aBottom = a.Position.Y + a.Size.Y;
        var bBottom = b.Position.Y + b.Size.Y;

        return Math.Max(0, Math.Min(aBottom, bBottom) - Math.Max(a.Position.Y, b.Position.Y));
    }

    private static void AdjustOverlap(ref FlyTextEvent a, ref FlyTextEvent b)
    {
        if (!IsOverlapping(ref a, ref b))
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

        (a.Animation.TimeElapsed < b.Animation.TimeElapsed ? a : b).Animation.Offset += new Vector2(0, GetOverlap(ref a, ref b));
    }

    private static Vector2 CalculateIconPosition(ref FlyTextEvent flyTextEvent, ref Vector2 textPos, ref Vector2 iconSize)
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