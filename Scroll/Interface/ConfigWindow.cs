namespace Scroll.Interface;

using System.Numerics;

using Dalamud.Interface.Windowing;

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
    }
}