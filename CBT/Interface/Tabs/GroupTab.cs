namespace CBT.Interface.Tabs;

using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using CBT.FlyText.Configuration;
using CBT.Helpers;
using CBT.Types;

/// <summary>
/// GroupTab configures settings for FlyTextCategory Groups.
/// </summary>
public class GroupTab : Tab<FlyTextCategory>
{
    private static readonly List<FlyTextCategory> GroupPickerValues = [.. FlyTextCategoryExtension.GetAllGroups()];

    private static FlyTextCategory currentGroup = FlyTextCategoryExtension.GetAllGroups().First();

    private static Dictionary<FlyTextCategory, FlyTextConfiguration>? tmpConfig;

    /// <inheritdoc/>
    public override string Name => TabKind.Group.ToString();

    /// <inheritdoc/>
    public override TabKind Kind => TabKind.Group;

    /// <inheritdoc/>
    protected override Dictionary<FlyTextCategory, FlyTextConfiguration> TmpConfig
    {
        get =>
            tmpConfig ??= Service.Configuration.FlyTextGroups
                .ToDictionary(
                    entry => entry.Key,
                    entry => new FlyTextConfiguration(entry.Value));

        set
        {
            tmpConfig = value;
            Service.Configuration.FlyTextGroups = tmpConfig;
        }
    }

    /// <inheritdoc />
    protected override Dictionary<FlyTextCategory, FlyTextConfiguration> Configuration
    {
        get => Service.Configuration.FlyTextGroups;
    }

    /// <inheritdoc/>
    protected override FlyTextCategory Current
    {
        get => currentGroup;
        set => currentGroup = value;
    }

    /// <inheritdoc/>
    public override void Draw()
    {
        Artist.DrawTitle("Group Configuration Settings");

        this.DrawCurrentConfigurations(GroupPickerValues);

        if (this.CurrentEnabled)
        {
            this.DrawFontConfigurations();
            this.DrawIconConfigurations();
            this.DrawAnimationConfigurations();
        }

        Artist.DrawSeperator();
        Artist.ColoredButton("Save##Group", sameLine: false, ButtonColors, this.OnSave);
    }

    /// <inheritdoc/>
    public override void OnClose()
    {
        this.ResetTmp();
        ResetCurrent();
    }

    /// <inheritdoc/>
    public override void ResetTmp()
    {
        this.TmpConfig = Service.Configuration.FlyTextGroups
            .ToDictionary(
                entry => entry.Key,
                entry => new FlyTextConfiguration(entry.Value));
    }

    private static void ResetCurrent()
    {
        currentGroup = FlyTextCategoryExtension.GetAllGroups().First();
    }

    private void OnSave()
    {
        this.TmpConfig.Keys.ToList().ForEach(group =>
        {
            if (this.TmpConfig.TryGetValue(group, out var currentConfig))
            {
                group.ForEachCategory(category =>
                {
                    Service.Configuration.FlyTextCategories[category] = new FlyTextConfiguration(currentConfig);

                    category.ForEachKind(kind => { Service.Configuration.FlyTextKinds[kind] = new FlyTextConfiguration(currentConfig); });
                });

                Service.Configuration.FlyTextGroups[group] = new FlyTextConfiguration(currentConfig);
            }
        });

        this.ResetTmp();
    }
}