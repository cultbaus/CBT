namespace CBT.Interface.Tabs;

using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using CBT.FlyText.Configuration;
using CBT.Helpers;
using CBT.Types;

/// <summary>
/// CatgeoryTab configures settings for FlyTextcategory Categories.
/// </summary>
public class CategoryTab : Tab<FlyTextCategory>
{
    private static readonly List<FlyTextCategory> CategoryPickerValues = [.. FlyTextCategoryExtension.GetAllCategories()];

    private static FlyTextCategory currentCategory = FlyTextCategoryExtension.GetAllCategories().First();

    private static Dictionary<FlyTextCategory, FlyTextConfiguration>? tmpConfig;

    /// <inheritdoc/>
    public override string Name => TabKind.Category.ToString();

    /// <inheritdoc/>
    public override TabKind Kind => TabKind.Category;

    /// <inheritdoc/>
    protected override Dictionary<FlyTextCategory, FlyTextConfiguration> TmpConfig
    {
        get =>
            tmpConfig ??= Service.Configuration.FlyTextCategories
                .ToDictionary(
                    entry => entry.Key,
                    entry => new FlyTextConfiguration(entry.Value));

        set
        {
            tmpConfig = value;
            Service.Configuration.FlyTextCategories = tmpConfig;
        }
    }

    /// <inheritdoc />
    protected override Dictionary<FlyTextCategory, FlyTextConfiguration> Configuration
    {
        get => Service.Configuration.FlyTextCategories;
    }

    /// <inheritdoc/>
    protected override FlyTextCategory Current
    {
        get => currentCategory;
        set => currentCategory = value;
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
        Artist.DrawTitle("Category Configuration Settings");

        this.DrawCurrentConfigurations(CategoryPickerValues);

        if (this.CurrentEnabled)
        {
            this.DrawFontConfigurations();
            this.DrawIconConfigurations();
            this.DrawAnimationConfigurations();
        }

        Artist.DrawSeperator();
        Artist.ColoredButton("Save##Category", sameLine: false, ButtonColors, this.OnSave);
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
        this.TmpConfig = Service.Configuration.FlyTextCategories
            .ToDictionary(
                entry => entry.Key,
                entry => new FlyTextConfiguration(entry.Value));
    }

    private static void ResetCurrent()
    {
        currentCategory = FlyTextCategoryExtension.GetAllCategories().First();
    }

    private void OnSave()
    {
        this.TmpConfig.Keys.ToList().ForEach(category =>
        {
            if (this.TmpConfig.TryGetValue(category, out var currentConfig))
            {
                category.ForEachKind(kind =>
                {
                    Service.Configuration.FlyTextKinds[kind] = new FlyTextConfiguration(currentConfig);
                });
                Service.Configuration.FlyTextCategories[category] = new FlyTextConfiguration(currentConfig);
            }
        });

        this.ResetTmp();
    }
}