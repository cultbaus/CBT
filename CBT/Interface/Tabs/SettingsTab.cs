namespace CBT.Interface.Tabs;

using System.Numerics;
using CBT.FlyText.Configuration;
using Dalamud.Interface.Utility.Raii;
using ImGuiNET;

/// <summary>
/// SettingsTab configures settings for FlyTextSettings Settingss.
/// </summary>
public class SettingsTab : Tab
{
    private static bool warningState = Service.Configuration.Warnings;

    /// <summary>
    /// Gets the Name of the Tab.
    /// </summary>
    public override string Name => TabKind.Settings.ToString();

    /// <summary>
    /// Gets the Settings of the Window drawn within the tab.
    /// </summary>
    public override TabKind Kind => TabKind.Settings;

    /// <inheritdoc/>
    public override void Draw()
    {
        using (Service.Fonts.Push(Defaults.DefaultFontName, 18f))
        {
            using (ImRaii.PushColor(ImGuiCol.Text, ImGui.GetColorU32(new Vector4(1, 1, 0, 1))))
            {
                ImGui.Text("Plugin configuration settings");
            }
        }
    }

    /// <inheritdoc/>
    public override void Selectable()
    {
        ImGui.Spacing();
        ImGui.Separator();
    }
}