namespace CBT.Interface.Tabs;

using System.Numerics;
using Dalamud.Interface.Utility.Raii;
using ImGuiNET;

/// <summary>
/// KindTab configures settings for FlyTextKind Kinds.
/// </summary>
internal class KindTab : Tab
{
    /// <summary>
    /// Gets the Name of the Tab.
    /// </summary>
    internal override string Name => TabKind.Kind.ToString();

    /// <summary>
    /// Gets the Kind of the Window drawn within the tab.
    /// </summary>
    internal override TabKind Kind => TabKind.Kind;

    /// <inheritdoc/>
    public override void Draw()
    {
        ImGui.Text("Hello from the Kind tab");
    }

    /// <inheritdoc/>
    public override void Selectable()
    {
    }
}