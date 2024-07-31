namespace Scroll.Interface;

using System.Collections.Generic;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;

using Scroll.FlyText;
using Scroll.FlyText.Types;

internal partial class ConfigWindow : Window
{
    private Dictionary<FlyTextKind, FlyTextConfiguration> groupConfigs = new();
    private Dictionary<FlyTextKind, FlyTextConfiguration> categoryConfigs = new();

    internal ConfigWindow()
        : base("Scroll Configuration##SCROLL_CONFIGURATION_WINDOW")
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
