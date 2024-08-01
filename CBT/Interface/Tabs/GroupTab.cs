namespace CBT.Interface.Tabs;

using ImGuiNET;

/// <summary>
/// GroupTab configures settings for FlyTextCategory Groups.
/// </summary>
internal class GroupTab : Tab
{
    /// <summary>
    /// Gets the Name of the Tab.
    /// </summary>
    internal override string Name => TabKind.Group.ToString();

    /// <summary>
    /// Gets the Kind of the Window drawn within the tab.
    /// </summary>
    internal override TabKind Kind => TabKind.Group;

    /// <inheritdoc/>
    public override void Draw()
    {
        ImGui.Text("Hello from the Group tab");
    }

    /// <inheritdoc/>
    public override void Selectable()
    {
    }
}