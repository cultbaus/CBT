// Here be dragons.

namespace CBT.Interface.Tabs;

using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using CBT.FlyText.Configuration;
using CBT.Helpers;
using CBT.Types;
using static FFXIVClientStructs.FFXIV.Client.UI.Info.InfoProxyCommonList.CharacterData;

/// <summary>
/// CatgeoryTab configures settings for FlyTextcategory Categories.
/// </summary>
public class CategoryTab : Tab<FlyTextCategory>
{
    private static readonly List<FlyTextCategory> CategoryPickerValues = [.. FlyTextCategoryExtension.GetAllCategories()];

    private static readonly List<string> FontPickerValues = [.. PluginConfiguration.Fonts.Keys];

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

    private bool CurrentCategoryEnabled
    {
        get => this.GetValue(config => config.Enabled);
        set => this.SetValue((config, val) => config.Enabled = val, value);
    }

    private string CurrentFont
    {
        get => this.GetValue(config => config.Font.Name);
        set => this.SetValue((config, val) => config.Font.Name = val, value);
    }

    private Vector4 CurrentFontColor
    {
        get => this.GetValue(config => config.Font.Color);
        set => this.SetValue((config, val) => config.Font.Color = val, value);
    }

    private float CurrentFontSize
    {
        get => this.GetValue(config => config.Font.Size);
        set => this.SetValue((config, val) => config.Font.Size = val, value);
    }

    private bool CurrentFontOutlineEnabled
    {
        get => this.GetValue(config => config.Font.Outline.Enabled);
        set => this.SetValue((config, val) => config.Font.Outline.Enabled = val, value);
    }

    private int CurrentFontOutlineThickness
    {
        get => this.GetValue(config => config.Font.Outline.Size);
        set => this.SetValue((config, val) => config.Font.Outline.Size = val, value);
    }

    private Vector4 CurrentFontOutlineColor
    {
        get => this.GetValue(config => config.Font.Outline.Color);
        set => this.SetValue((config, val) => config.Font.Outline.Color = val, value);
    }

    private bool CurrentIconEnabled
    {
        get => this.GetValue(config => config.Icon.Enabled);
        set => this.SetValue((config, val) => config.Icon.Enabled = val, value);
    }

    private float CurrentIconSize
    {
        get => this.GetValue(config => config.Icon.Size.X);
        set => this.SetValue((config, val) => config.Icon.Size = new Vector2(val, val), value);
    }

    private bool CurrentIconOutlineEnabled
    {
        get => this.GetValue(config => config.Icon.Outline.Enabled);
        set => this.SetValue((config, val) => config.Icon.Outline.Enabled = val, value);
    }

    private int CurrentIconOutlineThickness
    {
        get => this.GetValue(config => config.Icon.Outline.Size);
        set => this.SetValue((config, val) => config.Icon.Outline.Size = val, value);
    }

    private Vector4 CurrentIconOutlineColor
    {
        get => this.GetValue(config => config.Icon.Outline.Color);
        set => this.SetValue((config, val) => config.Icon.Outline.Color = val, value);
    }

    /// <inheritdoc/>
    public override void Draw()
    {
        Artist.DrawTitle("Category Configuration Settings");

        Artist.DrawSubTitle("Category Configurations");
        {
            Artist.DrawLabelPrefix("Select Category", sameLine: false);
            Artist.DrawSelectPicker("Category", sameLine: true, this.Current, CategoryPickerValues, category => { this.Current = category; });

            Artist.DrawLabelPrefix("Enabled", sameLine: false);
            Artist.Checkbox("Enabled", sameLine: true, this.CurrentCategoryEnabled, enabled => { this.CurrentCategoryEnabled = enabled; });
        }

        if (this.CurrentCategoryEnabled)
        {
            Artist.DrawSubTitle("Font Configurations");
            {
                Artist.DrawLabelPrefix("Select Font", sameLine: false);
                Artist.DrawSelectPicker("Font", sameLine: true, this.CurrentFont, FontPickerValues, font => { this.CurrentFont = font; });

                Artist.DrawLabelPrefix("Select Font Size", sameLine: false);
                Artist.DrawSelectPicker("Font Size", sameLine: true, this.CurrentFontSize, PluginConfiguration.Fonts[this.CurrentFont], size => { this.CurrentFontSize = size; }, 50f);

                Artist.DrawLabelPrefix("Select Font Color", sameLine: false);
                Artist.DrawColorPicker("Font Color", sameLine: true, this.CurrentFontColor, color => { this.CurrentFontColor = color; });
            }

            Artist.DrawSubTitle("Font Outline Configurations");
            {
                Artist.DrawLabelPrefix("Enable Font Outline", sameLine: false);
                Artist.Checkbox("Font Outline", sameLine: true, this.CurrentFontOutlineEnabled, enabled => { this.CurrentFontOutlineEnabled = enabled; });

                if (this.CurrentFontOutlineEnabled)
                {
                    Artist.DrawLabelPrefix("Select Font Outline Thickness", sameLine: false);
                    Artist.DrawInputInt("Font Outline Thickness", sameLine: true, this.CurrentFontOutlineThickness, 1, 5, thickness => { this.CurrentFontOutlineThickness = thickness; });

                    Artist.DrawLabelPrefix("Select Font Outline Color", sameLine: false);
                    Artist.DrawColorPicker("Font Outline Color", sameLine: true, this.CurrentFontOutlineColor, color => { this.CurrentFontOutlineColor = color; });
                }
            }

            Artist.DrawSubTitle("Icon Configurations");
            {
                Artist.DrawLabelPrefix("Enable Icon", sameLine: false);
                Artist.Checkbox("Enable Icon", sameLine: true, this.CurrentIconEnabled, enabled => { this.CurrentIconEnabled = enabled; });

                if (this.CurrentIconEnabled)
                {
                    Artist.DrawLabelPrefix("Select Icon Size", sameLine: false);
                    Artist.DrawInputInt("Icon Size", sameLine: true, (int)this.CurrentIconSize, 14, 32, size => { this.CurrentIconSize = size; });

                    Artist.DrawSubTitle("Icon Outline Configurations");
                    {
                        Artist.DrawLabelPrefix("Enable Icon Outline", sameLine: false);
                        Artist.Checkbox("Icon Outline", sameLine: true, this.CurrentIconOutlineEnabled, enabled => { this.CurrentIconOutlineEnabled = enabled; });

                        if (this.CurrentIconOutlineEnabled)
                        {
                            Artist.DrawLabelPrefix("Select Icon Outline Thickness", sameLine: false);
                            Artist.DrawInputInt("Outline Thickness", sameLine: true, this.CurrentIconOutlineThickness, 1, 5, thickness => { this.CurrentIconOutlineThickness = thickness; });

                            Artist.DrawLabelPrefix("Select Icon Outline Color", sameLine: false);
                            Artist.DrawColorPicker("Outline Color", sameLine: true, this.CurrentIconOutlineColor, color => { this.CurrentIconOutlineColor = color; });
                        }
                    }
                }
            }
        }

        Artist.DrawSeperator();
        Artist.ColoredButton("Save##Category", sameLine: false, ButtonColors, this.OnSave);
    }

    /// <inheritdoc/>
    public override void OnClose()
    {
        this.ResetTmp();
        currentCategory = FlyTextCategoryExtension.GetAllCategories().First();
    }

    private void OnSave()
    {
        this.TmpConfig.Keys.ToList().ForEach(category =>
        {
            if (this.TmpConfig.TryGetValue(category, out var currentConfig))
            {
                Service.PluginLog.Info($"Category: {currentConfig.Enabled}");

                category.ForEach(kind =>
                {
                    Service.Configuration.FlyTextKinds[kind] = new FlyTextConfiguration(currentConfig);
                });
                Service.Configuration.FlyTextCategories[category] = new FlyTextConfiguration(currentConfig);
            }
        });

        this.ResetTmp();
    }

    private void ResetTmp()
    {
        this.TmpConfig = Service.Configuration.FlyTextCategories
            .ToDictionary(
                entry => entry.Key,
                entry => new FlyTextConfiguration(entry.Value));
    }
}