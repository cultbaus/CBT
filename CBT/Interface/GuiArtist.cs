namespace CBT.Interface;

using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using CBT.FlyText.Configuration;
using CBT.Interface.Tabs;
using Dalamud.Interface;
using Dalamud.Interface.FontIdentifier;
using Dalamud.Interface.ImGuiFontChooserDialog;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using ImGuiNET;

/// <summary>
/// Artist is a set of utility methods for drawing ImGui thingies.
/// </summary>
public class GuiArtist
{
    private const float LongElementWidth = 250f;
    private const float ShortElementWidth = 50f;

    /// <summary>
    /// Draws an ImGUI Checkbox.
    /// </summary>
    /// <param name="label">Checkbox label.</param>
    /// <param name="sameLine">Should this element be on the same line.</param>
    /// <param name="value">Reference to the checkbox state.</param>
    /// <param name="onChanged">Action to handle changing states.</param>
    public static void Checkbox(string label, bool sameLine, bool value, Action<bool> onChanged)
    {
        if (sameLine)
        {
            ImGui.SameLine();
        }

        if (ImGui.Checkbox($"##{label}", ref value))
        {
            onChanged(value);
            Service.Configuration.Save();
        }
    }

    /// <summary>
    /// Draws a selectable label from the Tab state.
    /// </summary>
    /// <param name="tab">Tab to draw.</param>
    /// <param name="sameLine">Should this be on the same line.</param>
    /// <param name="selected">Whether the tab has been selected.</param>
    /// <param name="onClicked">What to do when clicked.</param>
    public static void SelectableTab(KindTab tab, bool sameLine, bool selected, Action<KindTab> onClicked)
    {
        if (sameLine)
        {
            ImGui.SameLine();
        }

        if (ImGui.Selectable($"{tab.Name}##KIND_TAB", selected))
        {
            onClicked(tab);
        }
    }

    /// <summary>
    /// Created a button with the assigned label and on-click handler.
    /// </summary>
    /// <param name="label">What the button says.</param>
    /// <param name="sameLine">Should this be on the same line.</param>
    /// <param name="onButtonPress">What the button does.</param>
    public static void Button(string label, bool sameLine, Action onButtonPress)
    {
        if (sameLine)
        {
            ImGui.SameLine();
        }

        if (ImGui.Button(label))
        {
            onButtonPress();
        }
    }

    /// <summary>
    /// Draws a styled button.
    /// </summary>
    /// <param name="label">Text label.</param>
    /// <param name="sameLine">Should this be on the same line.</param>
    /// <param name="colors">A list of style vars and colors to push.</param>
    /// <param name="onButtonPress">Action on push.</param>
    public static void ColoredButton(string label, bool sameLine, List<(ImGuiCol Style, Vector4 Color)> colors, Action onButtonPress)
    {
        colors.ForEach(color =>
        {
            ImGui.PushStyleColor(color.Style, color.Color);
        });

        if (sameLine)
        {
            ImGui.SameLine();
        }

        using (Service.Fonts.Push(Defaults.DefaultFontId, 24f))
        {
            if (ImGui.Button(label))
            {
                onButtonPress();
            }
        }

        ImGui.PopStyleColor(colors.Count);
    }

    /// <summary>
    /// Draws a child with a margin around the content area.
    /// </summary>
    /// <param name="childId">String ID for the child.</param>
    /// <param name="size">Size of the content area.</param>
    /// <param name="margin">Margin.</param>
    /// <param name="drawContent">Content action.</param>
    /// <param name="flags">ImGUI Window Flags.</param>
    public static void DrawChildWithMargin(string childId, Vector2 size, float margin, Action drawContent, ImGuiWindowFlags flags)
    {
        margin = Scale(margin);

        using (ImRaii.Child(childId, size, false))
        {
            ImGui.SetCursorPos(new Vector2(margin * 2, margin));

            var regionSize = ImGui.GetContentRegionAvail() - new Vector2(2 * margin, 2 * margin);
            using (ImRaii.Child($"##{childId}##CONTENT", regionSize, false, flags))
            {
                drawContent();
            }
        }
    }

    /// <summary>
    /// Draw Select Picker.
    /// </summary>
    /// <typeparam name="T">An enum.</typeparam>
    /// <param name="label">A label for the select picker.</param>
    /// <param name="sameLine">Should this be on the same line.</param>
    /// <param name="currentValue">The current value from which the preview is derived.</param>
    /// <param name="values">A list of values.</param>
    /// <param name="action">An action to take with kind T.</param>
    /// <param name="size">Size of the input width.</param>
    public static void DrawSelectPicker<T>(string label, bool sameLine, T currentValue, List<T> values, Action<T> action, float size = LongElementWidth)
    {
        size = Scale(size);

        if (sameLine)
        {
            ImGui.SameLine();
        }

        ImGui.SetNextItemWidth(size);

        using var combo = ImRaii.Combo($"##{label}", currentValue?.ToString()!);

        if (!combo)
        {
            return;
        }

        values.ForEach(el =>
        {
            if (el != null)
            {
                var isSelected = EqualityComparer<T>.Default.Equals(el, currentValue);

                if (ImGui.Selectable(el.ToString(), isSelected))
                {
                    action(el);
                }

                if (isSelected)
                {
                    ImGui.SetItemDefaultFocus();
                }
            }
        });
    }

    /// <summary>
    /// Draw Select Picker.
    /// </summary>
    /// <param name="label">A label for the select picker.</param>
    /// <param name="sameLine">Should this be on the same line.</param>
    /// <param name="currentFont">The current font.</param>
    /// <param name="action">An action to take with kind T.</param>
    /// <param name="size">Size of the input width.</param>
    public static void DrawFontPicker(string label, bool sameLine, IFontId currentFont, Action<IFontId> action, float size = LongElementWidth)
    {
        size = Scale(size);

        if (sameLine)
        {
            ImGui.SameLine();
        }

        ImGui.SetNextItemWidth(size);

        using var id = ImRaii.PushId($"##{label}");

        if (!ImGui.Button(currentFont.Family.EnglishName))
        {
            return;
        }

        var chooser = SingleFontChooserDialog.CreateAuto((UiBuilder)Service.Interface.UiBuilder);
        chooser.SelectedFont = new SingleFontSpec { FontId = currentFont };
        chooser?.ResultTask.ContinueWith(
            r =>
            {
                if (r.IsCompletedSuccessfully)
                {
                    action(r.Result.FontId);
                }
            });
    }

    /// <summary>
    /// Draws a color picker.
    /// </summary>
    /// <param name="label">String label for the picker.</param>
    /// <param name="sameLine">Should draw on the same line.</param>
    /// <param name="currentColor">Current color of the option to change, initial value.</param>
    /// <param name="action">Action to take with the new color.</param>
    /// <param name="size">Size of the picker.</param>
    public static void DrawColorPicker(string label, bool sameLine, Vector4 currentColor, Action<Vector4> action, float size = LongElementWidth)
    {
        size = Scale(size);

        var colorPicker = currentColor;

        if (sameLine)
        {
            ImGui.SameLine();
        }

        ImGui.SetNextItemWidth(size);

        if (ImGui.ColorEdit4($"##{label}", ref colorPicker))
        {
            action(colorPicker);
        }
    }

    /// <summary>
    /// Draws a separation.
    /// </summary>
    /// <param name="seperatorSize">Size in pixels to draw the space.</param>
    public static void DrawSeperator(float seperatorSize = 14f)
    {
        using (Service.Fonts.Push(Defaults.DefaultFontId, seperatorSize))
        {
            ImGui.Spacing();
            ImGui.Separator();
        }
    }

    /// <summary>
    /// Draws a title.
    /// </summary>
    /// <param name="titleText">The text of the title.</param>
    /// <param name="fontSize">The font size for the title.</param>
    public static void DrawTitle(string titleText, float fontSize = 24f)
    {
        using (Service.Fonts.Push(Defaults.DefaultFontId, fontSize))
        {
            using (ImRaii.PushColor(ImGuiCol.Text, ImGui.GetColorU32(new Vector4(1, 1, 0, 1))))
            {
                ImGui.Text(titleText);
                DrawSeperator();
            }
        }
    }

    /// <summary>
    /// Draws a title.
    /// </summary>
    /// <param name="titleText">The text of the title.</param>
    /// <param name="fontSize">The font size for the title.</param>
    public static void DrawSubTitle(string titleText, float fontSize = 18f)
    {
        using (Service.Fonts.Push(Defaults.DefaultFontId, fontSize))
        {
            using (ImRaii.PushColor(ImGuiCol.Text, ImGui.GetColorU32(new Vector4(0, 1, 1, 1))))
            {
                DrawSeperator();
                ImGui.Text(titleText);
                DrawSeperator();
            }
        }
    }

    /// <summary>
    /// Draws a warning labe.
    /// </summary>
    /// <param name="message">Warning message.</param>
    /// <param name="fontSize">Size of the warning.</param>
    public static void DrawWarning(string message, float fontSize = 16f)
    {
        using (Service.Fonts.Push(Defaults.DefaultFontId, fontSize))
        {
            using (ImRaii.PushColor(ImGuiCol.Text, ImGui.GetColorU32(new Vector4(1, 0, 0, 1))))
            {
                ImGui.Text(message);
                DrawSeperator();
            }
        }
    }

    /// <summary>
    /// Draws label prefix. Automatically inlines following element.
    /// </summary>
    /// <param name="titleText">The text of the title.</param>
    /// <param name="sameLine">Should appear on the same line.</param>
    /// <param name="fontSize">The font size for the title.</param>
    public static void DrawLabelPrefix(string titleText, bool sameLine, float fontSize = 14f)
    {
        if (sameLine)
        {
            ImGui.SameLine();
        }

        using (Service.Fonts.Push(Defaults.DefaultFontId, fontSize))
        {
            using (ImRaii.PushColor(ImGuiCol.Text, ImGui.GetColorU32(new Vector4(1, 1, 0, 1))))
            {
                ImGui.Text(titleText);
            }
        }
    }

    /// <summary>
    /// Draw an input int picker.
    /// </summary>
    /// <param name="label">Label for the componenet.</param>
    /// <param name="sameLine">Should appear on the same line.</param>
    /// <param name="value">Value to set.</param>
    /// <param name="min">Min value.</param>
    /// <param name="max">Max value.</param>
    /// <param name="onChange">What to do when input changes.</param>
    public static void DrawInputInt(string label, bool sameLine, int value, int min, int max, Action<int> onChange)
    {
        if (sameLine)
        {
            ImGui.SameLine();
        }

        ImGui.SetNextItemWidth(ShortElementWidth * 2);

        if (ImGui.InputInt($"##{label}", ref value, step: 1, step_fast: 2))
        {
            value = Math.Clamp(value, min, max);
            onChange(value);
        }
    }

    /// <summary>
    /// Draw an input string picker.
    /// </summary>
    /// <param name="label">Label for the component.</param>
    /// <param name="sameLine">Should appear on the same line.</param>
    /// <param name="value">Input original value.</param>
    /// <param name="onChange">What to do when input changes.</param>
    public static void DrawStringInput(string label, bool sameLine, string value, Action<string> onChange)
    {
        if (sameLine)
        {
            ImGui.SameLine();
        }

        byte[] buffer = new byte[256];
        Encoding.UTF8.GetBytes(value, 0, value.Length, buffer, 0);

        ImGui.SetNextItemWidth(Scale(ShortElementWidth * 3));

        if (ImGui.InputText($"##{label}", buffer, (uint)buffer.Length))
        {
            var input = Encoding.UTF8.GetString(buffer).TrimEnd('\0');
            onChange(input);
        }
    }

    /// <summary>
    /// Scale a float based on UI scale.
    /// </summary>
    /// <param name="f">Floating value before UI Scale adjustment.</param>
    /// <returns>Float that has been adjusted for UI scale.</returns>
    public static float Scale(float f)
        => f * ImGuiHelpers.GlobalScale;
}