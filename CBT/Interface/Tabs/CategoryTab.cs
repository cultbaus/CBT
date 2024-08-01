namespace CBT.Interface.Tabs;

using ImGuiNET;

/// <summary>
/// CategoryTab configures settings for FlyTextCategory Categorys.
/// </summary>
internal class CategoryTab : Tab
{
    /// <summary>
    /// Gets the Name of the Tab.
    /// </summary>
    internal override string Name => TabKind.Category.ToString();

    /// <summary>
    /// Gets the Kind of the Window drawn within the tab.
    /// </summary>
    internal override TabKind Kind => TabKind.Category;

    /// <inheritdoc/>
    public override void Draw()
    {
        ImGui.Text("Hello from the Category tab");
    }

    /// <inheritdoc/>
    public override void Selectable()
    {
    }
}