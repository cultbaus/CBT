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
/// CatgeoryTab configures settings for FlyTextcategory Categories.
/// </summary>
public class CategoryTab : Tab
{
    private static FlyTextCategory currentCategory = Enum.GetValues<FlyTextCategory>().Cast<FlyTextCategory>().FirstOrDefault(c => c.IsCategory());

    private static Dictionary<FlyTextCategory, FlyTextConfiguration> tmpConfig = new Dictionary<FlyTextCategory, FlyTextConfiguration>();

    /// <summary>
    /// Gets the Name of the Tab.
    /// </summary>
    public override string Name => TabKind.Category.ToString();

    /// <summary>
    /// Gets the Kind of the Window drawn within the tab.
    /// </summary>
    public override TabKind Kind => TabKind.Category;

    private static FlyTextCategory CurrentCategory
    {
        get => currentCategory;
        set => currentCategory = value;
    }

    private static bool CurrentcategoryEnabled
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
        Artist.DrawTabTitle("FlyText Categories Configuration");
        Artist.DrawSeperator();

        // Configuration options
        using (Service.Fonts.Push(Defaults.DefaultFontName, 16f))
        {
            Artist.DrawSeperator();

            if (DrawcategoryConfiguration(textPickerWidth))
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
        if (tmpConfig.TryGetValue(CurrentCategory, out var currentConfig))
        {
            CurrentCategory.ForEach(kind =>
            {
                Service.Configuration.FlyTextKinds[kind] = new FlyTextConfiguration(currentConfig);
            });
            Service.Configuration.FlyTextCategories[CurrentCategory] = new FlyTextConfiguration(currentConfig);
        }
    }

    private static T GetValue<T>(Func<FlyTextConfiguration, T> selector)
    {
        return tmpConfig.TryGetValue(CurrentCategory, out var currentConfig) ? selector(currentConfig) : selector(Service.Configuration.FlyTextCategories[CurrentCategory]);
    }

    private static void SetValue<T>(Action<FlyTextConfiguration, T> setter, T value)
    {
        if (!tmpConfig.TryGetValue(CurrentCategory, out var currentConfig))
        {
            currentConfig = new FlyTextConfiguration(Service.Configuration.FlyTextCategories[CurrentCategory]);
            tmpConfig[CurrentCategory] = currentConfig;
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

        Artist.StyledButton("Save##Category", colors, OnSave);
    }

    private static void DrawcategoryTitle()
    {
        using (Service.Fonts.Push(Defaults.DefaultFontName, 18f))
        {
            ImGui.Text("Category Configuration");
            ImGui.Spacing();
        }
    }

    private static void DrawCategoryPicker(float textPickerWidth)
    {
        using (ImRaii.PushColor(ImGuiCol.Text, ImGui.GetColorU32(new Vector4(1, 1, 0, 1))))
        {
            ImGui.Text("Select Category: ");
        }

        ImGui.SameLine();
        ImGui.SetNextItemWidth(textPickerWidth);
        Artist.DrawSelectPicker("Category", CurrentCategory, Enum.GetValues<FlyTextCategory>().Cast<FlyTextCategory>().Where(category => category.IsCategory()).ToList(), category => { CurrentCategory = category; });
    }

    private static bool DrawcategoryEnabledCheckbox()
    {
        ImGui.SameLine();
        Artist.Checkbox("Enabled", CurrentcategoryEnabled, enabled =>
        {
            CurrentcategoryEnabled = enabled;
        });

        return CurrentcategoryEnabled;
    }

    private static bool DrawcategoryConfiguration(float textPickerWidth)
    {
        DrawcategoryTitle();
        DrawCategoryPicker(textPickerWidth);
        return DrawcategoryEnabledCheckbox();
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