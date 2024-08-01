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
/// CategoryTab configures settings for FlyTextCategory Categorys.
/// </summary>
public class CategoryTab : Tab
{
    private static FlyTextCategory currentCategory =
        Enum.GetValues<FlyTextCategory>()
            .Cast<FlyTextCategory>()
            .FirstOrDefault(c => c.IsCategory());

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

    private static bool CurrentCategoryEnabled
    {
        get => Service.Configuration.FlyTextCategories[CurrentCategory].Enabled;
        set => Service.Configuration.FlyTextCategories[CurrentCategory].Enabled = value;
    }

    private static string CurrentFont
    {
        get => Service.Configuration.FlyTextCategories[CurrentCategory].Font.Name;
        set => Service.Configuration.FlyTextCategories[CurrentCategory].Font.Name = value;
    }

    private static Vector4 CurrentFontColor
    {
        get => Service.Configuration.FlyTextCategories[CurrentCategory].Font.Color;
        set => Service.Configuration.FlyTextCategories[CurrentCategory].Font.Color = value;
    }

    private static float CurrentFontSize
    {
        get => Service.Configuration.FlyTextCategories[CurrentCategory].Font.Size;
        set => Service.Configuration.FlyTextCategories[CurrentCategory].Font.Size = value;
    }

    private static bool CurrentOutlineEnabled
    {
        get => Service.Configuration.FlyTextCategories[CurrentCategory].Outline.Enabled;
        set => Service.Configuration.FlyTextCategories[CurrentCategory].Outline.Enabled = value;
    }

    private static int CurrentOutlineThickness
    {
        get => Service.Configuration.FlyTextCategories[CurrentCategory].Outline.Size;
        set => Service.Configuration.FlyTextCategories[CurrentCategory].Outline.Size = value;
    }

    private static Vector4 CurrentOutlineColor
    {
        get => Service.Configuration.FlyTextCategories[CurrentCategory].Outline.Color;
        set => Service.Configuration.FlyTextCategories[CurrentCategory].Outline.Color = value;
    }

    /// <inheritdoc/>
    public override void Draw()
    {
        using (Service.Fonts.Push(Defaults.DefaultFontName, 22f))
        {
            using (ImRaii.PushColor(ImGuiCol.Text, ImGui.GetColorU32(new Vector4(1, 1, 0, 1))))
            {
                ImGui.Text("FlyText Categories");
                ImGui.Spacing();
            }
        }

        using (Service.Fonts.Push(Defaults.DefaultFontName, 14f))
        {
            using (ImRaii.PushColor(ImGuiCol.Text, ImGui.GetColorU32(new Vector4(1, 0, 0, 1))))
            {
                ImGui.Text("Warning: changing settings here will overwrite Kinds!");
            }
        }

        ImGui.Separator();

        using (Service.Fonts.Push(Defaults.DefaultFontName, 16f))
        {
            var textPickerWidth = 250f;
            var numPickerWidth = 50f;

            ImGui.Spacing();
            ImGui.Separator();

            DrawCategoryConfiguration(textPickerWidth);

            if (CurrentCategoryEnabled)
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

    private static void DrawCategoryConfiguration(float textPickerWidth)
    {
        using (Service.Fonts.Push(Defaults.DefaultFontName, 18f))
        {
            ImGui.Text("Category Configuration");
            ImGui.Spacing();
        }

        {
            using (ImRaii.PushColor(ImGuiCol.Text, ImGui.GetColorU32(new Vector4(1, 1, 0, 1))))
            {
                ImGui.Text("Select Category: ");
            }

            ImGui.SameLine();
            ImGui.SetNextItemWidth(textPickerWidth);
            Artist.DrawSelectPicker("Category", CurrentCategory, Enum.GetValues<FlyTextCategory>().Cast<FlyTextCategory>().ToList().Where(category => category.IsCategory()).ToList(), category => { CurrentCategory = category; });

            ImGui.SameLine();
            Artist.Checkbox("Enabled", CurrentCategoryEnabled, enabled =>
            {
                CurrentCategory.ForEach(kind => Service.Configuration.FlyTextKinds[kind].Enabled = enabled);
                CurrentCategoryEnabled = enabled;
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
                CurrentCategory.ForEach(kind => Service.Configuration.FlyTextKinds[kind].Font.Name = name);
                CurrentFont = name;
            });
            ImGui.SameLine();
            ImGui.SetNextItemWidth(numPickerWidth);
            Artist.DrawSelectPicker("FontSize", CurrentFontSize, PluginConfiguration.Fonts[CurrentFont], size =>
            {
                CurrentCategory.ForEach(kind => Service.Configuration.FlyTextKinds[kind].Font.Size = size);
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
            CurrentCategory.ForEach(kind => Service.Configuration.FlyTextKinds[kind].Font.Color = color);
            CurrentFontColor = color;
        });

        using (ImRaii.PushColor(ImGuiCol.Text, ImGui.GetColorU32(new Vector4(1, 1, 0, 1))))
        {
            ImGui.Text("Select Font Outline: ");
        }

        ImGui.SameLine();
        Artist.Checkbox("Outline", CurrentOutlineEnabled, enabled =>
        {
            CurrentCategory.ForEach(kind => Service.Configuration.FlyTextKinds[kind].Outline.Enabled = enabled);
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

                CurrentCategory.ForEach(kind => Service.Configuration.FlyTextKinds[kind].Outline.Size = userInput);
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
                CurrentCategory.ForEach(kind => Service.Configuration.FlyTextKinds[kind].Outline.Color = color);
                CurrentOutlineColor = color;
            });
        }
    }
}