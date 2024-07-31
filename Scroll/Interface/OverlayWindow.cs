namespace Scroll.Interface;

using System;
using System.Numerics;

using Dalamud.Interface.Utility;
using Dalamud.Interface.Windowing;
using ImGuiNET;

internal partial class OverlayWindow : Window
{
    private static readonly ImGuiWindowFlags windowFlags =
        ImGuiWindowFlags.AlwaysUseWindowPadding
            | ImGuiWindowFlags.NoBackground
            | ImGuiWindowFlags.NoFocusOnAppearing
            | ImGuiWindowFlags.NoInputs
            | ImGuiWindowFlags.NoScrollbar
            | ImGuiWindowFlags.NoSavedSettings
            | ImGuiWindowFlags.NoTitleBar;
    private DateTime lastFrame = DateTime.Now;

    internal OverlayWindow()
        : base("Scroll Overlay Window##SCROLL_OVERLAY_WINDOW", windowFlags, true)
    {
        this.IsOpen = true;
        this.RespectCloseHotkey = false;
    }

    public override void PreDraw()
    {
        base.PreDraw();

        ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, Vector2.Zero);
        ImGui.SetNextWindowSize(ImGuiHelpers.MainViewport.Size);
        ImGuiHelpers.SetNextWindowPosRelativeMainViewport(Vector2.Zero);
    }

    public override void PostDraw()
    {
        base.PostDraw();

        ImGui.PopStyleVar();
    }

    public override void Draw()
    {
        var drawList = ImGui.GetWindowDrawList();
        var timeElapsed = TimeSince(this.lastFrame);

        Service.Manager.Update(timeElapsed);
        Service.Manager.Draw(drawList);
    }

    internal float TimeSince(DateTime lastFrame)
    {
        float timeElapsed = (float)(DateTime.Now - lastFrame).TotalSeconds;
        this.lastFrame = DateTime.Now;

        return timeElapsed;
    }
}