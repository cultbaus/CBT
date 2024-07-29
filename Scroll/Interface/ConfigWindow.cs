namespace Scroll.Interface;

using System.Numerics;

using Dalamud.Interface.Windowing;
using ImGuiNET;

internal class ConfigWindow : Window
{
    internal ConfigWindow()
        : base("Scroll Configuration##SCROLL_CONFIGURATION_WINDOW")
    {
        this.RespectCloseHotkey = true;
        this.SizeCondition = ImGuiNET.ImGuiCond.FirstUseEver;
        this.Size = new Vector2(720, 480);
    }

    public override void Draw()
    {
        ImGui.Indent();

        using (Service.Fonts.Push(Service.Configuration.DefaultFont, Service.Configuration.DefaultFontSize))
        {
            ImGui.Text("Scroll plugin configuration coming soon!");
        }
    }
}