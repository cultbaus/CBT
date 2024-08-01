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
/// GroupTab configures settings for FlyTextGroup Groups.
/// </summary>
public class GroupTab : Tab
{
    private static FlyTextCategory currentGroup =
        Enum.GetValues<FlyTextCategory>()
            .Cast<FlyTextCategory>()
            .FirstOrDefault(c => c.IsGroup());

    /// <summary>
    /// Gets the Name of the Tab.
    /// </summary>
    public override string Name => TabKind.Group.ToString();

    /// <summary>
    /// Gets the Kind of the Window drawn within the tab.
    /// </summary>
    public override TabKind Kind => TabKind.Group;

    private static FlyTextCategory CurrentGroup
    {
        get => currentGroup;
        set => currentGroup = value;
    }

    private static bool CurrentGroupEnabled
    {
        get => Service.Configuration.FlyTextGroups[CurrentGroup].Enabled;
        set => Service.Configuration.FlyTextGroups[CurrentGroup].Enabled = value;
    }

    private static string CurrentFont
    {
        get => Service.Configuration.FlyTextGroups[CurrentGroup].Font.Name;
        set => Service.Configuration.FlyTextGroups[CurrentGroup].Font.Name = value;
    }

    private static Vector4 CurrentFontColor
    {
        get => Service.Configuration.FlyTextGroups[CurrentGroup].Font.Color;
        set => Service.Configuration.FlyTextGroups[CurrentGroup].Font.Color = value;
    }

    private static float CurrentFontSize
    {
        get => Service.Configuration.FlyTextGroups[CurrentGroup].Font.Size;
        set => Service.Configuration.FlyTextGroups[CurrentGroup].Font.Size = value;
    }

    private static bool CurrentOutlineEnabled
    {
        get => Service.Configuration.FlyTextGroups[CurrentGroup].Outline.Enabled;
        set => Service.Configuration.FlyTextGroups[CurrentGroup].Outline.Enabled = value;
    }

    private static int CurrentOutlineThickness
    {
        get => Service.Configuration.FlyTextGroups[CurrentGroup].Outline.Size;
        set => Service.Configuration.FlyTextGroups[CurrentGroup].Outline.Size = value;
    }

    private static Vector4 CurrentOutlineColor
    {
        get => Service.Configuration.FlyTextGroups[CurrentGroup].Outline.Color;
        set => Service.Configuration.FlyTextGroups[CurrentGroup].Outline.Color = value;
    }

    /// <inheritdoc/>
    public override void Draw()
    {
        using (Service.Fonts.Push(Defaults.DefaultFontName, 22f))
        {
            using (ImRaii.PushColor(ImGuiCol.Text, ImGui.GetColorU32(new Vector4(1, 1, 0, 1))))
            {
                ImGui.Text("FlyText Groups");
                ImGui.Spacing();
            }
        }

        using (Service.Fonts.Push(Defaults.DefaultFontName, 14f))
        {
            using (ImRaii.PushColor(ImGuiCol.Text, ImGui.GetColorU32(new Vector4(1, 0, 0, 1))))
            {
                ImGui.Text("Warning: changing settings here will overwrite Categories & Kinds!");
            }
        }

        ImGui.Separator();

        using (Service.Fonts.Push(Defaults.DefaultFontName, 16f))
        {
            var textPickerWidth = 250f;
            var numPickerWidth = 50f;

            ImGui.Spacing();
            ImGui.Separator();

            DrawGroupConfiguration(textPickerWidth);

            if (CurrentGroupEnabled)
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

    private static void DrawGroupConfiguration(float textPickerWidth)
    {
        using (Service.Fonts.Push(Defaults.DefaultFontName, 18f))
        {
            ImGui.Text("Group Configuration");
            ImGui.Spacing();
        }

        {
            using (ImRaii.PushColor(ImGuiCol.Text, ImGui.GetColorU32(new Vector4(1, 1, 0, 1))))
            {
                ImGui.Text("Select Group: ");
            }

            ImGui.SameLine();
            ImGui.SetNextItemWidth(textPickerWidth);
            Artist.DrawSelectPicker("Group", CurrentGroup, Enum.GetValues<FlyTextCategory>().Cast<FlyTextCategory>().ToList().Where(group => group.IsGroup()).ToList(), group => { CurrentGroup = group; });

            ImGui.SameLine();
            Artist.Checkbox("Enabled", CurrentGroupEnabled, enabled =>
            {
                CurrentGroup.ForEach(kind => Service.Configuration.FlyTextKinds[kind].Enabled = enabled);
                CurrentGroupEnabled = enabled;
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
                CurrentGroup.ForEach(kind => Service.Configuration.FlyTextKinds[kind].Font.Name = name);
                CurrentFont = name;
            });
            ImGui.SameLine();
            ImGui.SetNextItemWidth(numPickerWidth);
            Artist.DrawSelectPicker("FontSize", CurrentFontSize, PluginConfiguration.Fonts[CurrentFont], size =>
            {
                CurrentGroup.ForEach(kind => Service.Configuration.FlyTextKinds[kind].Font.Size = size);
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
            CurrentGroup.ForEach(kind => Service.Configuration.FlyTextKinds[kind].Font.Color = color);
            CurrentFontColor = color;
        });

        using (ImRaii.PushColor(ImGuiCol.Text, ImGui.GetColorU32(new Vector4(1, 1, 0, 1))))
        {
            ImGui.Text("Select Font Outline: ");
        }

        ImGui.SameLine();
        Artist.Checkbox("Outline", CurrentOutlineEnabled, enabled =>
        {
            CurrentGroup.ForEach(kind => Service.Configuration.FlyTextKinds[kind].Outline.Enabled = enabled);
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

                CurrentGroup.ForEach(kind => Service.Configuration.FlyTextKinds[kind].Outline.Size = userInput);
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
                CurrentGroup.ForEach(kind => Service.Configuration.FlyTextKinds[kind].Outline.Color = color);
                CurrentOutlineColor = color;
            });
        }
    }
}