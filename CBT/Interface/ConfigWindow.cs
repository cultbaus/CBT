namespace CBT.Interface;

using System.IO;
using System.Numerics;
using CBT.FlyText.Configuration;
using CBT.Interface.Tabs;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;
using ImGuiNET;

/// <summary>
/// ConfigWindow is the primary configuration GUI for CBT.
/// </summary>
public partial class ConfigWindow : Window
{
    private readonly Tab tab = new KindTab();

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigWindow"/> class.
    /// </summary>
    /// <param name="name">Name of the <see cref="Plugin"/>.</param>
    public ConfigWindow(string name)
        : base($"{name} - Cultbaus Battle Text##{name}_CONFIGURATION_WINDOW", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        this.RespectCloseHotkey = true;
        this.SizeCondition = ImGuiCond.FirstUseEver;
        this.Size = new Vector2(900, 450);
    }

    /// <inheritdoc/>
    public override void Draw()
    {
        using (Service.Fonts.Push(Defaults.DefaultFontId, 14f))
        {
            using (ImRaii.PushStyle(ImGuiStyleVar.CellPadding, new Vector2(GuiArtist.Scale(5), GuiArtist.Scale(30))))
            {
                using (ImRaii.Table("##CONFIGURATION_TABLE", 2, ImGuiTableFlags.BordersInnerV))
                {
                    ImGui.TableSetupColumn("##LEFT_CONFIG_COLUMN", ImGuiTableColumnFlags.WidthFixed, GuiArtist.Scale(165f));
                    DrawLeftColumn();

                    ImGui.TableSetupColumn("##RIGHT_CONFIG_COLUMN", ImGuiTableColumnFlags.WidthStretch);
                    this.DrawRightColumn();
                }
            }
        }
    }

    /// <inheritdoc/>
    public override void OnOpen()
        => base.OnOpen();

    /// <inheritdoc/>
    public override void OnClose()
    {
        this.tab.OnClose();
        Service.Configuration.Save();
    }

    private static void DrawLeftColumn()
    {
        var regionSize = ImGui.GetContentRegionAvail();

        ImGui.TableNextColumn();
        using (ImRaii.Child("##LEFT_COLUMN_CHILD", ImGui.GetContentRegionAvail(), false, ImGuiWindowFlags.NoDecoration))
        {
            DrawLogo(new Vector2(GuiArtist.Scale(125f), GuiArtist.Scale(125f)));

            // Do not scale this
            using (ImRaii.PushStyle(ImGuiStyleVar.SelectableTextAlign, new Vector2(0.5f, 0.5f)))
            {
                // TODO: fixme
            }
        }
    }

    private static void DrawLogo(Vector2 imageSize)
    {
        var imagePath = Path.Combine(Service.Interface.AssemblyLocation.DirectoryName!, "Data\\icon.png");
        var logoImage = Service.TextureProvider.GetFromFile(imagePath).GetWrapOrDefault();

        var regionSize = ImGui.GetContentRegionAvail();

        using (ImRaii.Child("##LOGO", regionSize with { Y = imageSize.Y }, false, ImGuiWindowFlags.NoDecoration))
        {
            if (logoImage != null)
            {
                var imagePadding = (regionSize.X - imageSize.X) / 2;

                ImGui.SetCursorPosX(imagePadding);
                ImGui.Image(logoImage.ImGuiHandle, imageSize);

                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("You know what it stands for 8^)");
                    ImGui.EndTooltip();
                }
            }
            else
            {
                ImGui.Text("Unable to load logo image.");
            }
        }
    }

    private void DrawRightColumn()
    {
        void DrawContent()
        {
            this.tab?.Draw();
        }

        ImGui.TableNextColumn();
        GuiArtist.DrawChildWithMargin("##RIGHT_COLUMN_CHILD", Vector2.Zero, GuiArtist.Scale(5f), DrawContent, ImGuiWindowFlags.None);
    }
}