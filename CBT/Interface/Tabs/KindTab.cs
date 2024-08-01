// Here be dragons.

namespace CBT.Interface.Tabs;

using System;
using System.Linq;
using System.Numerics;
using CBT.FlyText;
using CBT.FlyText.Configuration;
using CBT.FlyText.Types;
using Dalamud.Interface.Utility.Raii;
using ImGuiNET;

/// <summary>
/// KindTab configures settings for FlyTextKinds.
/// </summary>
public class KindTab : Tab
{
    private static FlyTextKind currentKind =
        Enum.GetValues<FlyTextKind>()
            .Cast<FlyTextKind>()
            .First();

    /// <summary>
    /// Gets the Name of the Tab.
    /// </summary>
    public override string Name => TabKind.Kind.ToString();

    /// <summary>
    /// Gets the Kind of the Window drawn within the tab.
    /// </summary>
    public override TabKind Kind => TabKind.Kind;

    private static FlyTextKind CurrentKind
    {
        get => currentKind;
        set => currentKind = value;
    }

    private static bool CurrentKindEnabled
    {
        get => Service.Configuration.FlyTextKinds[CurrentKind].Enabled;
        set => Service.Configuration.FlyTextKinds[CurrentKind].Enabled = value;
    }

    private static string CurrentFont
    {
        get => Service.Configuration.FlyTextKinds[CurrentKind].Font.Name;
        set => Service.Configuration.FlyTextKinds[CurrentKind].Font.Name = value;
    }

    private static Vector4 CurrentFontColor
    {
        get => Service.Configuration.FlyTextKinds[CurrentKind].Font.Color;
        set => Service.Configuration.FlyTextKinds[CurrentKind].Font.Color = value;
    }

    private static float CurrentFontSize
    {
        get => Service.Configuration.FlyTextKinds[CurrentKind].Font.Size;
        set => Service.Configuration.FlyTextKinds[CurrentKind].Font.Size = value;
    }

    private static bool CurrentOutlineEnabled
    {
        get => Service.Configuration.FlyTextKinds[CurrentKind].Outline.Enabled;
        set => Service.Configuration.FlyTextKinds[CurrentKind].Outline.Enabled = value;
    }

    private static int CurrentOutlineThickness
    {
        get => Service.Configuration.FlyTextKinds[CurrentKind].Outline.Size;
        set => Service.Configuration.FlyTextKinds[CurrentKind].Outline.Size = value;
    }

    private static Vector4 CurrentOutlineColor
    {
        get => Service.Configuration.FlyTextKinds[CurrentKind].Outline.Color;
        set => Service.Configuration.FlyTextKinds[CurrentKind].Outline.Color = value;
    }

    /// <inheritdoc/>
    public override void Draw()
    {
        using (Service.Fonts.Push(Defaults.DefaultFontName, 22f))
        {
            using (ImRaii.PushColor(ImGuiCol.Text, ImGui.GetColorU32(new Vector4(1, 1, 0, 1))))
            {
                ImGui.Text("FlyText Kinds");
                ImGui.Spacing();
            }
        }

        using (Service.Fonts.Push(Defaults.DefaultFontName, 14f))
        {
            using (ImRaii.PushColor(ImGuiCol.Text, ImGui.GetColorU32(new Vector4(1, 0, 0, 1))))
            {
                ImGui.Text(" ");
            }
        }

        ImGui.Separator();

        using (Service.Fonts.Push(Defaults.DefaultFontName, 16f))
        {
            var textPickerWidth = 250f;
            var numPickerWidth = 50f;

            ImGui.Spacing();
            ImGui.Separator();

            DrawKindConfiguration(textPickerWidth);

            if (CurrentKindEnabled)
            {
                ImGui.Spacing();
                ImGui.Separator();

                DrawFontConfiguration(textPickerWidth, numPickerWidth);
            }
        }
    }

    /// <inheritdoc/>
    public override void Selectable()
    {
    }

    private static void DrawKindConfiguration(float textPickerWidth)
    {
        using (Service.Fonts.Push(Defaults.DefaultFontName, 18f))
        {
            ImGui.Text("Kind Configuration");
            ImGui.Spacing();
        }

        {
            using (ImRaii.PushColor(ImGuiCol.Text, ImGui.GetColorU32(new Vector4(1, 1, 0, 1))))
            {
                ImGui.Text("Select Kind: ");
            }

            ImGui.SameLine();
            ImGui.SetNextItemWidth(textPickerWidth);
            Artist.DrawSelectPicker("Kind", CurrentKind, Enum.GetValues<FlyTextKind>().Cast<FlyTextKind>().ToList().ToList(), kind => { CurrentKind = kind; });

            ImGui.SameLine();
            Artist.Checkbox("Enabled", CurrentKindEnabled, enabled =>
            {
                CurrentKindEnabled = enabled;
            });
        }
    }

    private static void DrawFontConfiguration(float textPickerWidth, float numPickerWidth)
    {
        using (Service.Fonts.Push(Defaults.DefaultFontName, 18f))
        {
            ImGui.Text("Font Configuration");
            ImGui.Spacing();
        }

        {
            using (ImRaii.PushColor(ImGuiCol.Text, ImGui.GetColorU32(new Vector4(1, 1, 0, 1))))
            {
                ImGui.Text("Select Font: ");
            }

            ImGui.SameLine();
            ImGui.SetNextItemWidth(textPickerWidth);
            Artist.DrawSelectPicker("Font", CurrentFont, [.. PluginConfiguration.Fonts.Keys], name =>
            {
                CurrentFont = name;
            });
            ImGui.SameLine();
            ImGui.SetNextItemWidth(numPickerWidth);
            Artist.DrawSelectPicker("FontSize", CurrentFontSize, PluginConfiguration.Fonts[CurrentFont], size =>
            {
                CurrentFontSize = size;
            });
        }

        using (ImRaii.PushColor(ImGuiCol.Text, ImGui.GetColorU32(new Vector4(1, 1, 0, 1))))
        {
            ImGui.Text("Select Font Color: ");
        }

        ImGui.SameLine();
        ImGui.SetNextItemWidth(textPickerWidth);
        Artist.DrawColorPicker("Color##Font", CurrentFontColor, color =>
        {
            CurrentFontColor = color;
        });

        using (ImRaii.PushColor(ImGuiCol.Text, ImGui.GetColorU32(new Vector4(1, 1, 0, 1))))
        {
            ImGui.Text("Select Font Outline: ");
        }

        ImGui.SameLine();
        Artist.Checkbox("Outline", CurrentOutlineEnabled, enabled =>
        {
            CurrentOutlineEnabled = enabled;
        });

        if (CurrentOutlineEnabled)
        {
            using (ImRaii.PushColor(ImGuiCol.Text, ImGui.GetColorU32(new Vector4(1, 1, 0, 1))))
            {
                ImGui.Text("Select Outline Thickness: ");
            }

            ImGui.SameLine();
            ImGui.SetNextItemWidth(numPickerWidth * 2);
            var userInput = CurrentOutlineThickness;
            if (ImGui.InputInt("##Outline Thickness", ref userInput, step: 1, step_fast: 2))
            {
                userInput = Math.Clamp(userInput, 1, 5);

                CurrentOutlineThickness = userInput;
            }

            using (ImRaii.PushColor(ImGuiCol.Text, ImGui.GetColorU32(new Vector4(1, 1, 0, 1))))
            {
                ImGui.Text("Select Outline Color: ");
            }

            ImGui.SameLine();
            ImGui.SetNextItemWidth(textPickerWidth);
            Artist.DrawColorPicker("Color##Outline", CurrentOutlineColor, color =>
            {
                CurrentOutlineColor = color;
            });
        }
    }
}