namespace CBT.Interface;

using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;

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
    }

    public override void OnOpen()
        => base.OnOpen();

    public override void OnClose()
    {
        base.OnClose();

        Service.Configuration.Save();
    }
}
