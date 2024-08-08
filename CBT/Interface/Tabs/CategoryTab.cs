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
    public override void Draw()
    {
        Artist.DrawTitle("Category Configuration Settings");

        this.DrawCurrentConfigurations(CategoryPickerValues);

        if (this.CurrentEnabled)
        {
            this.DrawFontConfigurations();

            if (this.Current == FlyTextCategory.AbilityDamage)
            {
                this.DrawPositionalPickers();
            }

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