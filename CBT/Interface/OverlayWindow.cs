namespace CBT.Interface;

using System;
using System.Numerics;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Windowing;
using ImGuiNET;

/// <summary>
/// OverlayWindow is the primary canvas for CBT FlyTextEvents.
/// </summary>
public class OverlayWindow : Window
{
    private static readonly ImGuiWindowFlags WindowFlags =
        ImGuiWindowFlags.AlwaysUseWindowPadding
            | ImGuiWindowFlags.NoBackground
            | ImGuiWindowFlags.NoFocusOnAppearing
            | ImGuiWindowFlags.NoInputs
            | ImGuiWindowFlags.NoScrollbar
            | ImGuiWindowFlags.NoSavedSettings
            | ImGuiWindowFlags.NoTitleBar;

    private DateTime lastFrame = DateTime.Now;

    /// <summary>
    /// Initializes a new instance of the <see cref="OverlayWindow"/> class.
    /// </summary>
    public OverlayWindow()
        : base("CBT Overlay Window##CBT_OVERLAY_WINDOW", WindowFlags, true)
    {
        this.IsOpen = true;
        this.RespectCloseHotkey = false;
    }

    /// <inheritdoc/>
    public override void PreDraw()
    {
        base.PreDraw();

        ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, Vector2.Zero);
        ImGui.SetNextWindowSize(ImGuiHelpers.MainViewport.Size);
        ImGuiHelpers.SetNextWindowPosRelativeMainViewport(Vector2.Zero);
    }

    /// <inheritdoc/>
    public override void PostDraw()
    {
        base.PostDraw();

        ImGui.PopStyleVar();
    }

    /// <inheritdoc/>
    public override void Draw()
    {
        var drawList = ImGui.GetWindowDrawList();
        var timeElapsed = this.TimeSince(this.lastFrame);

        Service.Manager.Update(timeElapsed);
        Service.Manager.Draw(drawList);
    }

    /// <summary>
    /// Calculates the time since the last frame.
    /// </summary>
    /// <param name="lastFrame">Previous DateTime persisted to the OverlayWindow.</param>
    /// <returns>Returns the Time Elapsed since the last Draw.</returns>
    public float TimeSince(DateTime lastFrame)
    {
        float timeElapsed = (float)(DateTime.Now - lastFrame).TotalSeconds;
        this.lastFrame = DateTime.Now;

        return timeElapsed;
    }
}