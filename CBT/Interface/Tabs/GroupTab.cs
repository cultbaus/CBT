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
    protected override bool CurrentEnabled
    {
        get => this.GetValue(config => config.Enabled);
        set => this.SetValue((config, val) => config.Enabled = val, value);
    }

    /// <inheritdoc/>
    protected override bool CurrentEnabledForSelf
    {
        get => this.GetValue(config => config.Filter.Self);
        set => this.SetValue((config, val) => config.Filter.Self = val, value);
    }

    /// <inheritdoc/>
    protected override bool CurrentEnabledForEnemy
    {
        get => this.GetValue(config => config.Filter.Enemy);
        set => this.SetValue((config, val) => config.Filter.Enemy = val, value);
    }

    /// <inheritdoc/>
    protected override bool CurrentEnabledForParty
    {
        get => this.GetValue(config => config.Filter.Party);
        set => this.SetValue((config, val) => config.Filter.Party = val, value);
    }

    /// <inheritdoc/>
    protected override string CurrentFont
    {
        get => this.GetValue(config => config.Font.Name);
        set => this.SetValue((config, val) => config.Font.Name = val, value);
    }

    /// <inheritdoc/>
    protected override Vector4 CurrentFontColor
    {
        get => this.GetValue(config => config.Font.Color);
        set => this.SetValue((config, val) => config.Font.Color = val, value);
    }

    /// <inheritdoc/>
    protected override float CurrentFontSize
    {
        get => this.GetValue(config => config.Font.Size);
        set => this.SetValue((config, val) => config.Font.Size = val, value);
    }

    /// <inheritdoc/>
    protected override bool CurrentFontOutlineEnabled
    {
        get => this.GetValue(config => config.Font.Outline.Enabled);
        set => this.SetValue((config, val) => config.Font.Outline.Enabled = val, value);
    }

    /// <inheritdoc/>
    protected override int CurrentFontOutlineThickness
    {
        get => this.GetValue(config => config.Font.Outline.Size);
        set => this.SetValue((config, val) => config.Font.Outline.Size = val, value);
    }

    /// <inheritdoc/>
    protected override Vector4 CurrentFontOutlineColor
    {
        get => this.GetValue(config => config.Font.Outline.Color);
        set => this.SetValue((config, val) => config.Font.Outline.Color = val, value);
    }

    /// <inheritdoc/>
    protected override bool CurrentIconEnabled
    {
        get => this.GetValue(config => config.Icon.Enabled);
        set => this.SetValue((config, val) => config.Icon.Enabled = val, value);
    }

    /// <inheritdoc/>
    protected override float CurrentIconSize
    {
        get => this.GetValue(config => config.Icon.Size.X);
        set => this.SetValue((config, val) => config.Icon.Size = new Vector2(val, val), value);
    }

    /// <inheritdoc/>
    protected override bool CurrentIconOutlineEnabled
    {
        get => this.GetValue(config => config.Icon.Outline.Enabled);
        set => this.SetValue((config, val) => config.Icon.Outline.Enabled = val, value);
    }

    /// <inheritdoc/>
    protected override int CurrentIconOutlineThickness
    {
        get => this.GetValue(config => config.Icon.Outline.Size);
        set => this.SetValue((config, val) => config.Icon.Outline.Size = val, value);
    }

    /// <inheritdoc/>
    protected override Vector4 CurrentIconOutlineColor
    {
        get => this.GetValue(config => config.Icon.Outline.Color);
        set => this.SetValue((config, val) => config.Icon.Outline.Color = val, value);
    }

    /// <inheritdoc/>
    protected override bool CurrentAnimationReversed
    {
        get => this.GetValue(config => config.Animation.Reversed);
        set => this.SetValue((config, val) => config.Animation.Reversed = val, value);
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