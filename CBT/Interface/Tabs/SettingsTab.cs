namespace CBT.Interface.Tabs;

using System;
using System.Collections.Generic;
using System.Linq;
using CBT.Helpers;

/// <summary>
/// Settings tab.
/// </summary>
public class SettingsTab : ITab
{
    private static readonly Dictionary<string, int> SortOrder = new()
    {
        { TabKind.Kind.ToString(), 1 },
        { TabKind.Category.ToString(), 2 },
        { TabKind.Group.ToString(), 3 },
        { TabKind.Settings.ToString(), 4 },
    };

    static SettingsTab()
    {
        Tabs = [new KindTab(), new SettingsTab()];

        if (Service.Configuration.Options[TabKind.Group.ToString()])
        {
            AddTab(new GroupTab());
        }

        if (Service.Configuration.Options[TabKind.Category.ToString()])
        {
            AddTab(new CategoryTab());
        }

        SortTabs();
    }

    /// <summary>
    /// Gets the enabled for configuration.
    /// </summary>
    public static List<ITab> Tabs { get; private set; }

    /// <inheritdoc/>
    public string Name => TabKind.Settings.ToString();

    /// <inheritdoc/>
    public TabKind Kind => TabKind.Settings;

    /// <summary>
    /// Draw the Settings tab.
    /// </summary>
    public void Draw()
    {
        SortTabs();

        Artist.DrawTitle("CBT Configuration Settings");

        Artist.DrawSubTitle("Configuration Options");

        Artist.DrawLabelPrefix("Enable Native FlyText Passthrough", sameLine: false);
        if (Service.Configuration.Options.TryGetValue(GlobalOption.NativeUnhandled.ToString(), out var nativeUnhandled))
        {
            Artist.Checkbox("Enable Native FlyText Passthrough", sameLine: true, nativeUnhandled, enabled =>
            {
                Service.Configuration.Options[GlobalOption.NativeUnhandled.ToString()] = enabled;
                Service.Configuration.Save();
            });
        }

        Artist.DrawLabelPrefix("Unlock Player Anchor", sameLine: false);
        if (Service.Configuration.Options.TryGetValue(GlobalOption.PlayerAnchor.ToString(), out var playerAnchor))
        {
            Artist.Checkbox("Unlock Player Anchor", sameLine: true, playerAnchor, enabled =>
            {
                Service.Configuration.Options[GlobalOption.PlayerAnchor.ToString()] = enabled;
                Service.Configuration.Save();
            });
        }

        Artist.DrawSubTitle("Bulk Configurations");
        Artist.DrawWarning("Warning: These configurations will overwrite individual FlyText Kind configurations.", 14f);
        Artist.DrawLabelPrefix("Enable Category Configurations", sameLine: false);
        if (Service.Configuration.Options.TryGetValue(TabKind.Category.ToString(), out var categoryConfig))
        {
            Artist.Checkbox("EnableCategoryConfigurations_Checkbox", sameLine: true, categoryConfig, enabled =>
            {
                Service.Configuration.Options[TabKind.Category.ToString()] = enabled;

                if (!enabled)
                {
                    var tab = Tabs.FirstOrDefault(tab => tab.Name == TabKind.Category.ToString());
                    if (tab != null)
                    {
                        RemoveTab(tab);
                    }
                }
                else
                {
                    AddTab(new CategoryTab());
                }

                Service.Configuration.Save();
            });
        }

        Artist.DrawWarning("Warning: These configurations will overwrite individual FlyText Kind & FlyText Category configurations.", 14f);
        Artist.DrawLabelPrefix("Enable Group Configurations", sameLine: false);
        if (Service.Configuration.Options.TryGetValue(TabKind.Group.ToString(), out var groupConfig))
        {
            Artist.Checkbox("EnableGroupConfigurations_Checkbox", sameLine: true, groupConfig, enabled =>
            {
                Service.Configuration.Options[TabKind.Group.ToString()] = enabled;

                if (!enabled)
                {
                    var tab = Tabs.FirstOrDefault(tab => tab.Name == TabKind.Group.ToString());
                    if (tab != null)
                    {
                        RemoveTab(tab);
                    }
                }
                else
                {
                    AddTab(new GroupTab());
                }

                Service.Configuration.Save();
            });
        }
    }

    /// <inheritdoc/>
    public void OnClose()
    {
        Service.Configuration.Save();
    }

    /// <inheritdoc/>
    public void ResetTmp()
    {
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Tabs.Clear();

        GC.SuppressFinalize(this);
    }

    private static void RemoveTab(ITab tab)
    {
        Tabs.Remove(tab);
    }

    private static void AddTab(ITab tab)
    {
        Tabs.Add(tab);
    }

    private static void SortTabs()
    {
        Tabs = [.. Tabs.OrderBy(e => SortOrder.ContainsKey(e.Name) ? SortOrder[e.Name] : int.MaxValue)];
    }
}