namespace CBT.Interface;

using System.Numerics;
using CBT.FlyText;
using CBT.FlyText.Configuration;
using Dalamud.Interface.Windowing;
using ImGuiNET;

/// <summary>
/// ConfigWindow is the primary configuration GUI for CBT.
/// </summary>
internal partial class ConfigWindow : Window
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigWindow"/> class.
    /// </summary>
    /// <param name="name">Name of the <see cref="Plugin"/>.</param>
    internal ConfigWindow(string name)
        : base($"{name} Configuration##{name}_CONFIGURATION_WINDOW")
    {
        this.RespectCloseHotkey = true;
        this.SizeCondition = ImGuiCond.FirstUseEver;
        this.Size = new Vector2(720, 480);
    }

    /// <inheritdoc/>
    public override void Draw()
    {
        using (Service.Fonts.Push(Defaults.DefaultFontName, 24.0f))
        {
            var windowSize = ImGui.GetWindowSize();

            static void DrawText(string content, Vector2 windowSize)
            {
                var textSize = ImGui.CalcTextSize(content);

                var posX = (windowSize.X - textSize.X) * 0.5f;
                var posY = (windowSize.Y - textSize.Y) * 0.5f;

                ImGui.SetCursorPos(new Vector2(posX, posY));
                ImGui.Text(content);
            }

            DrawText("Configure defaults in PluginConfiguration.cs for now", windowSize);
        }
    }

    /// <inheritdoc/>
    public override void OnOpen()
        => base.OnOpen();

    /// <inheritdoc/>
    public override void OnClose()
    {
        base.OnClose();

        Service.Configuration.Save();
    }
}