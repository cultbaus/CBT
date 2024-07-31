namespace CBT.Interface;

using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;

using CBT.FlyText;

internal partial class ConfigWindow : Window
{
    internal ConfigWindow(string name)
        : base($"{name} Configuration##{name}_CONFIGURATION_WINDOW")
    {
        this.RespectCloseHotkey = true;
        this.SizeCondition = ImGuiCond.FirstUseEver;
        this.Size = new Vector2(720, 480);
    }

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

    public override void OnOpen()
        => base.OnOpen();

    public override void OnClose()
    {
        base.OnClose();

        Service.Configuration.Save();
    }
}
