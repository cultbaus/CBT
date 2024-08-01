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
internal partial class ConfigWindow : Window
{
    private static readonly List<Tab> Tabs = new List<Tab>()
    {
        new KindTab(),
        new CategoryTab(),
        new GroupTab(),
        new SettingsTab(),
    };

    private static TabKind currentTab = TabKind.Kind;
    private static TabKind clickedTab = TabKind.Kind;
    private static bool shouldPromptConfirmation = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigWindow"/> class.
    /// </summary>
    /// <param name="name">Name of the <see cref="Plugin"/>.</param>
    internal ConfigWindow(string name)
        : base($"{name} - Cultbaus Battle Text##{name}_CONFIGURATION_WINDOW", ImGuiWindowFlags.NoScrollbar)
    {
        this.RespectCloseHotkey = true;
        this.SizeCondition = ImGuiCond.FirstUseEver;
        this.Size = new Vector2(720, 480);
    }

    /// <inheritdoc/>
    public override void Draw()
    {
        using (Service.Fonts.Push(Defaults.DefaultFontName, 14f))
        {
            using (ImRaii.PushStyle(ImGuiStyleVar.CellPadding, new Vector2(5, 0)))
            {
                using (ImRaii.Table("##CONFIGURATION_TABLE", 2, ImGuiTableFlags.BordersInnerV))
                {
                    ImGui.TableSetupColumn("##LEFT_CONFIG_COLUMN", ImGuiTableColumnFlags.WidthFixed, Artist.Scale(165f));
                    DrawLeftColumn();

                    ImGui.TableSetupColumn("##RIGHT_CONFIG_COLUMN", ImGuiTableColumnFlags.WidthStretch);
                    DrawRightColumn();
                }
            }

            DrawConfirmationModal(shouldPromptConfirmation && PluginConfiguration.Warnings);
        }
    }

    /// <inheritdoc/>
    public override void OnOpen()
        => base.OnOpen();

    /// <inheritdoc/>
    public override void OnClose()
    {
        base.OnClose();

        Service.Configuration.Save();
    }

    private static void DrawConfirmationModal(bool shouldDisplayModal)
    {
        if (shouldDisplayModal)
        {
            ImGui.SetNextWindowSize(new Vector2(300, 150), ImGuiCond.FirstUseEver);
            ImGui.SetNextWindowPos(new Vector2((ImGui.GetIO().DisplaySize.X - 300) / 2, (ImGui.GetIO().DisplaySize.Y - 150) / 2), ImGuiCond.FirstUseEver);

            ImGui.Begin("Confirm Tab Switch", ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse);
            {
                ImGui.Text("Warning! Changing these configurations will overwrite more granular settings, do you wish to proceed?");

                Artist.Button("Yes", () =>
                {
                    currentTab = clickedTab;
                    clickedTab = default;
                    shouldPromptConfirmation = false;
                });

                ImGui.SameLine();

                Artist.Button("No", () =>
                {
                    clickedTab = default;
                    shouldPromptConfirmation = false;
                });
            }

            ImGui.End();
        }
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
                    tab.Selectable();
                    Artist.SelectableTab(tab, currentTab == tab.Kind, t =>
                    {
                        clickedTab = t.Kind;

                        shouldPromptConfirmation = t.Kind.HasFlag(TabKind.Warning);
                        currentTab = shouldPromptConfirmation && PluginConfiguration.Warnings ? currentTab : clickedTab;
                    });
                });
            }
        }
    }

    private static void DrawLogo(Vector2 imageSize)
    {
        var imagePath = Path.Combine(Service.Interface.AssemblyLocation.DirectoryName!, "Data\\icon.png");
        var logoImage = Service.TextureProvider.GetFromFile(imagePath).GetWrapOrDefault();

        var regionSize = ImGui.GetContentRegionAvail();

        using (ImRaii.Child("##LOGO", regionSize with { Y = 125f }, false, ImGuiWindowFlags.NoDecoration))
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