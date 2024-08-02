// Here be dragons.

namespace CBT.Interface.Tabs;

using System;
using System.Collections.Generic;
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
    private static FlyTextCategory currentGroup = Enum.GetValues<FlyTextCategory>().Cast<FlyTextCategory>().FirstOrDefault(c => c.IsGroup());

    private static Dictionary<FlyTextCategory, FlyTextConfiguration> tmpConfig = new Dictionary<FlyTextCategory, FlyTextConfiguration>();

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
        get => GetValue(config => config.Enabled);
        set => SetValue((config, val) => config.Enabled = val, value);
    }

    private static string CurrentFont
    {
        get => GetValue(config => config.Font.Name);
        set => SetValue((config, val) => config.Font.Name = val, value);
    }

    private static Vector4 CurrentFontColor
    {
        get => GetValue(config => config.Font.Color);
        set => SetValue((config, val) => config.Font.Color = val, value);
    }

    private static float CurrentFontSize
    {
        get => GetValue(config => config.Font.Size);
        set => SetValue((config, val) => config.Font.Size = val, value);
    }

    private static bool CurrentOutlineEnabled
    {
        get => GetValue(config => config.Outline.Enabled);
        set => SetValue((config, val) => config.Outline.Enabled = val, value);
    }

    private static int CurrentOutlineThickness
    {
        get => GetValue(config => config.Outline.Size);
        set => SetValue((config, val) => config.Outline.Size = val, value);
    }

    private static Vector4 CurrentOutlineColor
    {
        get => GetValue(config => config.Outline.Color);
        set => SetValue((config, val) => config.Outline.Color = val, value);
    }

    /// <inheritdoc/>
    public override void Draw()
    {
        var textPickerWidth = 250f;
        var numPickerWidth = 50f;

        // Title bar
        Artist.DrawTabTitle("FlyText Groups Configuration");
        Artist.DrawSeperator();

        // Configuration options
        using (Service.Fonts.Push(Defaults.DefaultFontName, 16f))
        {
            Artist.DrawSeperator();

            if (DrawGroupConfiguration(textPickerWidth))
            {
                Artist.DrawSeperator();
                DrawFontConfiguration(textPickerWidth, numPickerWidth);
            }
        }

        DrawSaveButton();
    }

    /// <inheritdoc/>
    public override void OnClose()
    {
        tmpConfig.Clear();
    }

    private static void OnSave()
    {
        if (tmpConfig.TryGetValue(CurrentGroup, out var currentConfig))
        {
            CurrentGroup.ForEach(kind =>
            {
                Service.Configuration.FlyTextKinds[kind] = new FlyTextConfiguration(currentConfig);
            });
            Service.Configuration.FlyTextGroups[CurrentGroup] = new FlyTextConfiguration(currentConfig);
        }
    }

    private static T GetValue<T>(Func<FlyTextConfiguration, T> selector)
    {
        return tmpConfig.TryGetValue(CurrentGroup, out var currentConfig) ? selector(currentConfig) : selector(Service.Configuration.FlyTextGroups[CurrentGroup]);
    }

    private static void SetValue<T>(Action<FlyTextConfiguration, T> setter, T value)
    {
        if (!tmpConfig.TryGetValue(CurrentGroup, out var currentConfig))
        {
            currentConfig = new FlyTextConfiguration(Service.Configuration.FlyTextGroups[CurrentGroup]);
            tmpConfig[CurrentGroup] = currentConfig;
        }

        setter(currentConfig, value);
    }

    private static void DrawSaveButton()
    {
        var colors = new List<(ImGuiCol Style, Vector4 Color)>
    {
        (ImGuiCol.Text, new Vector4(1, 1, 1, 1)),
        (ImGuiCol.Button, new Vector4(206 / 255f, 39 / 255f, 187 / 255f, 1.0f)),
        (ImGuiCol.ButtonHovered, new Vector4(39 / 255f, 187 / 255f, 206 / 255f, 1.0f)),
        (ImGuiCol.ButtonActive, new Vector4(1, 1, 0, 1)),
    };

        Artist.StyledButton("Save##Group", colors, OnSave);
    }

    private static void DrawGroupTitle()
    {
        using (Service.Fonts.Push(Defaults.DefaultFontName, 18f))
        {
            ImGui.Text("Group Configuration");
            ImGui.Spacing();
        }
    }

    private static void DrawGroupPicker(float textPickerWidth)
    {
        using (ImRaii.PushColor(ImGuiCol.Text, ImGui.GetColorU32(new Vector4(1, 1, 0, 1))))
        {
            ImGui.Text("Select Group: ");
        }

        ImGui.SameLine();
        ImGui.SetNextItemWidth(textPickerWidth);
        Artist.DrawSelectPicker("Group", CurrentGroup, Enum.GetValues<FlyTextCategory>().Cast<FlyTextCategory>().Where(group => group.IsGroup()).ToList(), group => { CurrentGroup = group; });
    }

    private static bool DrawGroupEnabledCheckbox()
    {
        ImGui.SameLine();
        Artist.Checkbox("Enabled", CurrentGroupEnabled, enabled =>
        {
            CurrentGroupEnabled = enabled;
        });

        return CurrentGroupEnabled;
    }

    private static bool DrawGroupConfiguration(float textPickerWidth)
    {
        DrawGroupTitle();
        DrawGroupPicker(textPickerWidth);
        return DrawGroupEnabledCheckbox();
    }

    private static void DrawFontTitle()
    {
        using (Service.Fonts.Push(Defaults.DefaultFontName, 18f))
        {
            ImGui.Text("Font Configuration");
            ImGui.Spacing();
        }
    }

    private static void DrawFontPicker(float textPickerWidth)
    {
        using (ImRaii.PushColor(ImGuiCol.Text, ImGui.GetColorU32(new Vector4(1, 1, 0, 1))))
        {
            ImGui.Text("Select Font: ");
        }

        ImGui.SameLine();
        ImGui.SetNextItemWidth(textPickerWidth);
        Artist.DrawSelectPicker("Font", CurrentFont, PluginConfiguration.Fonts.Keys.ToList(), name =>
        {
            CurrentFont = name;
        });
    }

    private static void DrawFontSizePicker(float numPickerWidth)
    {
        ImGui.SameLine();
        ImGui.SetNextItemWidth(numPickerWidth);
        Artist.DrawSelectPicker("FontSize", CurrentFontSize, PluginConfiguration.Fonts[CurrentFont], size =>
        {
            CurrentFontSize = size;
        });
    }

    private static void DrawFontColorPicker(float textPickerWidth)
    {
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
    }

    private static bool DrawFontOutlinePicker()
    {
        using (ImRaii.PushColor(ImGuiCol.Text, ImGui.GetColorU32(new Vector4(1, 1, 0, 1))))
        {
            ImGui.Text("Select Font Outline: ");
        }

        ImGui.SameLine();
        Artist.Checkbox("Outline", CurrentOutlineEnabled, enabled =>
        {
            CurrentOutlineEnabled = enabled;
        });

        return CurrentOutlineEnabled;
    }

    private static void DrawFontOutlineThicknessPicker(float numPickerWidth)
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
    }

    private static void DrawFontOutlineColorPicker(float textPickerWidth)
    {
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

    private static void DrawFontConfiguration(float textPickerWidth, float numPickerWidth)
    {
        DrawFontTitle();
        DrawFontPicker(textPickerWidth);
        DrawFontSizePicker(numPickerWidth);
        DrawFontColorPicker(textPickerWidth);
        if (DrawFontOutlinePicker())
        {
            DrawFontOutlineThicknessPicker(numPickerWidth);
            DrawFontOutlineColorPicker(textPickerWidth);
        }
    }
}