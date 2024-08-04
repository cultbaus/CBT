namespace CBT.Interface;

using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    private static readonly List<Tab> Tabs = new List<Tab>()
    {
        new KindTab(),
        new CategoryTab(),
        new GroupTab(),
    };

    private static TabKind currentTab = TabKind.Kind;

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
        using (Service.Fonts.Push(Defaults.DefaultFontName, 14f))
        {
            using (ImRaii.PushStyle(ImGuiStyleVar.CellPadding, new Vector2(5, 30)))
            {
                using (ImRaii.Table("##CONFIGURATION_TABLE", 2, ImGuiTableFlags.BordersInnerV))
                {
                    ImGui.TableSetupColumn("##LEFT_CONFIG_COLUMN", ImGuiTableColumnFlags.WidthFixed, Artist.Scale(165f));
                    DrawLeftColumn();

                    ImGui.TableSetupColumn("##RIGHT_CONFIG_COLUMN", ImGuiTableColumnFlags.WidthStretch);
                    DrawRightColumn();
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
        Tabs.ForEach(tab => tab.OnClose());

        Service.Configuration.Save();
    }

    private static void DrawLeftColumn()
    {
        ImGui.TableNextColumn();
        using (ImRaii.Child("##LEFT_COLUMN_CHILD", ImGui.GetContentRegionAvail(), false, ImGuiWindowFlags.NoDecoration))
        {
            DrawLogo(new Vector2(Artist.Scale(125f), Artist.Scale(67f)));

            using (ImRaii.PushStyle(ImGuiStyleVar.SelectableTextAlign, new Vector2(0.5f, 0.5f)))
            {
                Tabs.ForEach(tab =>
                {
                    var isSelected = currentTab == tab.Kind;

                    using (ImRaii.PushColor(ImGuiCol.Text, ImGui.GetColorU32(new Vector4(1, 1, 0, 1)), isSelected))
                    {
                        using (Service.Fonts.Push(Defaults.DefaultFontName, 16f))
                        {
                            Artist.SelectableTab(tab, isSelected, t => { currentTab = t.Kind; });
                        }
                    }
                });
            }
        }
    }

    private static void DrawLogo(Vector2 imageSize)
    {
        var imagePath = Path.Combine(Service.Interface.AssemblyLocation.DirectoryName!, "Data\\icon.png");
        var logoImage = Service.TextureProvider.GetFromFile(imagePath).GetWrapOrDefault();

        var regionSize = ImGui.GetContentRegionAvail();

        using (ImRaii.Child("##LOGO", regionSize with { Y = 67f * 1.25f }, false, ImGuiWindowFlags.NoDecoration))
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

    private static void DrawRightColumn()
    {
        ImGui.TableNextColumn();
        Artist.DrawChildWithMargin("##RIGHT_COLUMN_CHILD", Vector2.Zero, Artist.Scale(5f), () =>
        {
            Tabs.FirstOrDefault(tab => tab.Kind == currentTab)?.Draw();
        });
    }
}