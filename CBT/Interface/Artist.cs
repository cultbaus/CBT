namespace CBT.Interface;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using CBT.Interface.Tabs;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using ImGuiNET;

/// <summary>
/// Artist is a set of utility methods for drawing ImGui thingies.
/// </summary>
public class Artist
{
    /// <summary>
    /// Draws an ImGUI Checkbox.
    /// </summary>
    /// <param name="label">Checkbox label.</param>
    /// <param name="value">Reference to the checkbox state.</param>
    /// <param name="onChanged">Action to handle changing states.</param>
    public static void Checkbox(string label, bool value, Action<bool>? onChanged = null)
    {
        if (ImGui.Checkbox(label, ref value) && onChanged != null)
        {
            onChanged(value);
        }
    }

    /// <summary>
    /// Draws a selectable label from the Tab state.
    /// </summary>
    /// <param name="tab">Tab to draw.</param>
    /// <param name="selected">Whether the tab has been selected.</param>
    /// <param name="onClicked">What to do when clicked.</param>
    public static void SelectableTab(Tab tab, bool selected, Action<Tab> onClicked)
    {
        if (ImGui.Selectable($"{tab.Name}##{tab.Kind}_WINDOW", selected))
        {
            onClicked(tab);
        }
    }

    /// <summary>
    /// Created a button with the assigned label and on-click handler.
    /// </summary>
    /// <param name="label">What the button says.</param>
    /// <param name="onButtonPress">What the button does.</param>
    public static void Button(string label, Action onButtonPress)
    {
        if (ImGui.Button(label))
        {
            onButtonPress();
        }
    }

    /// <summary>
    /// Draws a child with a margin around the content area.
    /// </summary>
    /// <param name="childId">String ID for the child.</param>
    /// <param name="size">Size of the content area.</param>
    /// <param name="margin">Margin.</param>
    /// <param name="drawContent">Content action.</param>
    public static void DrawChildWithMargin(string childId, Vector2 size, float margin, Action drawContent)
    {
        using (ImRaii.Child(childId, size, false))
        {
            ImGui.SetCursorPos(new Vector2(margin * 2, margin));

            var regionSize = ImGui.GetContentRegionAvail() - new Vector2(2 * margin, 2 * margin);
            using (ImRaii.Child(childId + "_CONTENT", regionSize, false, ImGuiWindowFlags.NoDecoration))
            {
                drawContent();
            }
        }
    }

    /// <summary>
    /// Draw Select Picker for the FlyTextKind/FlyTextCategory options.
    /// </summary>
    /// <typeparam name="T">An enum.</typeparam>
    /// <param name="label">A label for the select picker.</param>
    /// <param name="currentValue">The current value from which the preview is derived.</param>
    /// <param name="values">A list of values.</param>
    /// <param name="action">An action to take with kind T.</param>
    public static void DrawSelectPicker<T>(string label, T currentValue, List<T> values, Action<T> action)
    {
        using var combo = ImRaii.Combo(string.Empty + $"##_{label}", currentValue?.ToString()!);

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
                    currentValue = el;
                }

                if (isSelected)
                {
                    ImGui.SetItemDefaultFocus();
                }
            }
        });

        action(currentValue);
    }

    /// <summary>
    /// Draws a color picker.
    /// </summary>
    /// <param name="label">String label for the picker.</param>
    /// <param name="currentColor">Current color of the option to change, initial value.</param>
    /// <param name="action">Action to take with the new color.</param>
    public static void DrawColorPicker(string label, Vector4 currentColor, Action<Vector4> action)
    {
        var colorPicker = currentColor;

        if (ImGui.ColorEdit4(label, ref colorPicker))
        {
            currentColor = colorPicker;
        }

        action(currentColor);
    }

    /// <summary>
    /// Scale a float based on UI scale.
    /// </summary>
    /// <param name="f">Floating value before UI Scale adjustment.</param>
    /// <returns>Float that has been adjusted for UI scale.</returns>
    public static float Scale(float f)
        => f * ImGuiHelpers.GlobalScale * (Service.Interface.UiBuilder.DefaultFontSpec.SizePt / 12f);
}