namespace CBT.Interface.Tabs;

using System;
using System.Collections.Generic;
using System.Numerics;
using CBT.FlyText.Configuration;
using CBT.Helpers;
using ImGuiNET;

/// <summary>
/// TabKind represents the different types of configuration windows.
/// </summary>
[Flags]
public enum TabKind
{
    /// <summary>
    /// Tab to configure Group settings.
    /// </summary>
    Group = 1 << 10,

    /// <summary>
    /// Tab to configure Category settings.
    /// </summary>
    Category = 1 << 11,

    /// <summary>
    /// Tab to configure Kind settings.
    /// </summary>
    Kind = 1 << 12,

    /// <summary>
    /// Tab to configure Configuration settings.
    /// </summary>
    Settings = 1 << 20,
}

/// <summary>
/// The tab interface.
/// </summary>
public interface ITab : IDisposable
{
    /// <summary>
    /// Gets the name.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the tab kind.
    /// </summary>
    TabKind Kind { get; }

    /// <summary>
    /// Draws.
    /// </summary>
    void Draw();

    /// <summary>
    /// Handles closing.
    /// </summary>
    void OnClose();

    /// <summary>
    /// Resets the temporary state.
    /// </summary>
    void ResetTmp();
}

/// <summary>
/// Tab class.
/// </summary>
/// <typeparam name="TKey">The type of the tab to configure for.</typeparam>
public abstract class Tab<TKey> : ITab
where TKey : notnull
{
    /// <summary>
    /// ButtonColors for save button styling.
    /// </summary>
    protected static readonly List<(ImGuiCol Style, Vector4 Color)> ButtonColors =
    [
        (ImGuiCol.Text, new Vector4(1, 1, 1, 1)),
        (ImGuiCol.Button, new Vector4(206 / 255f, 39 / 255f, 187 / 255f, 1.0f)),
        (ImGuiCol.ButtonHovered, new Vector4(39 / 255f, 187 / 255f, 206 / 255f, 1.0f)),
        (ImGuiCol.ButtonActive, new Vector4(1, 1, 0, 1)),
    ];

    private static readonly List<string> FontPickerValues = [.. PluginConfiguration.Fonts.Keys];

    /// <summary>
    /// Gets the Name of the window.
    /// </summary>
    public abstract string Name { get; }

    /// <summary>
    /// Gets the Kind of the window.
    /// </summary>
    public abstract TabKind Kind { get; }

    /// <summary>
    /// Gets or sets the current type being configured.
    /// </summary>
    protected abstract TKey Current { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether current is enabled.
    /// </summary>
    protected abstract bool CurrentEnabled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether enabled for self.
    /// </summary>
    protected abstract bool CurrentEnabledForSelf { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether enabled for enemies.
    /// </summary>
    protected abstract bool CurrentEnabledForEnemy { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether enabled for party members.
    /// </summary>
    protected abstract bool CurrentEnabledForParty { get; set; }

    /// <summary>
    /// Gets or sets the current font.
    /// </summary>
    protected abstract string CurrentFont { get; set; }

    /// <summary>
    /// Gets or sets the current font color.
    /// </summary>
    protected abstract Vector4 CurrentFontColor { get; set; }

    /// <summary>
    /// Gets or sets the current font size.
    /// </summary>
    protected abstract float CurrentFontSize { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the font outline is enabled.
    /// </summary>
    protected abstract bool CurrentFontOutlineEnabled { get; set; }

    /// <summary>
    /// Gets or sets the font outline thickness.
    /// </summary>
    protected abstract int CurrentFontOutlineThickness { get; set; }

    /// <summary>
    /// Gets or sets the font outline color.
    /// </summary>
    protected abstract Vector4 CurrentFontOutlineColor { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the icon is enabled.
    /// </summary>
    protected abstract bool CurrentIconEnabled { get; set; }

    /// <summary>
    /// Gets or sets the icon size.
    /// </summary>
    protected abstract float CurrentIconSize { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether icon outline is enabled.
    /// </summary>
    protected abstract bool CurrentIconOutlineEnabled { get; set; }

    /// <summary>
    /// Gets or sets the icon outline thickness.
    /// </summary>
    protected abstract int CurrentIconOutlineThickness { get; set; }

    /// <summary>
    /// Gets or sets the icon outline color.
    /// </summary>
    protected abstract Vector4 CurrentIconOutlineColor { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the animation is reversed.
    /// </summary>
    protected abstract bool CurrentAnimationReversed { get; set; }

    /// <summary>
    /// Gets or sets a cache of the current configuration items.
    /// </summary>
    protected abstract Dictionary<TKey, FlyTextConfiguration> TmpConfig { get; set; }

    /// <summary>
    /// Gets the persisted configuration items.
    /// </summary>
    protected abstract Dictionary<TKey, FlyTextConfiguration> Configuration { get; }

    /// <summary>
    /// Dispose.
    /// </summary>
    public virtual void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Draws the Window associated with a Tab.
    /// </summary>
    public abstract void Draw();

    /// <summary>
    /// What to do on Close event.
    /// </summary>
    public abstract void OnClose();

    /// <summary>
    /// Reset tmp state.
    /// </summary>
    public abstract void ResetTmp();

    /// <summary>
    /// Get a configuration value.
    /// </summary>
    /// <typeparam name="T">The type being configured.</typeparam>
    /// <param name="selector">The means to select the field being retrieved.</param>
    /// <returns>The value.</returns>
    protected T GetValue<T>(Func<FlyTextConfiguration, T> selector)
    {
        return this.TmpConfig.TryGetValue(this.Current, out var currentConfig)
            ? selector(currentConfig)
            : selector(this.Configuration[this.Current]);
    }

    /// <summary>
    /// Set a configuration value.
    /// </summary>
    /// <typeparam name="T">The type being configured.</typeparam>
    /// <param name="setter">The means to set the field.</param>
    /// <param name="value">The value to set.</param>
    protected void SetValue<T>(Action<FlyTextConfiguration, T> setter, T value)
    {
        if (!this.TmpConfig.TryGetValue(this.Current, out var currentConfig))
        {
            currentConfig = new FlyTextConfiguration(this.Configuration[this.Current]);
            this.TmpConfig[this.Current] = currentConfig;
        }

        setter(currentConfig, value);
    }

    /// <summary>
    /// Draw configuration options current elements.
    /// </summary>
    /// <param name="allKinds">All kinds to be configured. Can be groups, categories, or kinds.</param>
    protected void DrawCurrentConfigurations(List<TKey> allKinds)
    {
        Artist.DrawSubTitle($"{this.Name} Configurations");
        {
            Artist.DrawLabelPrefix($"Select {this.Name}", sameLine: false);
            Artist.DrawSelectPicker($"Select_{this.Name}", sameLine: true, this.Current, allKinds, category => { this.Current = category; });

            Artist.DrawLabelPrefix("Enabled", sameLine: false);
            Artist.Checkbox($"Enabled_{this.Name}", sameLine: true, this.CurrentEnabled, enabled => { this.CurrentEnabled = enabled; });

            if (this.CurrentEnabled)
            {
                Artist.DrawLabelPrefix("Enabled On Self", sameLine: false);
                Artist.Checkbox($"Enabled On Self_{this.Name}", sameLine: true, this.CurrentEnabledForSelf, enabled => { this.CurrentEnabledForSelf = enabled; });

                Artist.DrawLabelPrefix("Enabled On Enemy", sameLine: false);
                Artist.Checkbox($"Enabled On Enemy_{this.Name}", sameLine: true, this.CurrentEnabledForEnemy, enabled => { this.CurrentEnabledForEnemy = enabled; });

                Artist.DrawLabelPrefix("Enabled On Party", sameLine: false);
                Artist.Checkbox($"Enabled On Party_{this.Name}", sameLine: true, this.CurrentEnabledForParty, enabled => { this.CurrentEnabledForParty = enabled; });
            }
        }
    }

    /// <summary>
    /// Draw font configurations.
    /// </summary>
    protected void DrawFontConfigurations()
    {
        Artist.DrawSubTitle("Font Configurations");
        {
            Artist.DrawLabelPrefix("Select Font", sameLine: false);
            Artist.DrawSelectPicker($"Font_{this.Name}", sameLine: true, this.CurrentFont, FontPickerValues, font => { this.CurrentFont = font; });

            Artist.DrawLabelPrefix("Select Font Size", sameLine: false);
            Artist.DrawSelectPicker($"Font Size_{this.Name}", sameLine: true, this.CurrentFontSize, PluginConfiguration.Fonts[this.CurrentFont], size => { this.CurrentFontSize = size; }, 50f);

            Artist.DrawLabelPrefix("Select Font Color", sameLine: false);
            Artist.DrawColorPicker($"Font Color_{this.Name}", sameLine: true, this.CurrentFontColor, color => { this.CurrentFontColor = color; });

            Artist.DrawSubTitle("Font Outline Configurations");
            {
                Artist.DrawLabelPrefix("Enable Font Outline", sameLine: false);
                Artist.Checkbox($"Font Outline_{this.Name}", sameLine: true, this.CurrentFontOutlineEnabled, enabled => { this.CurrentFontOutlineEnabled = enabled; });

                if (this.CurrentFontOutlineEnabled)
                {
                    Artist.DrawLabelPrefix("Select Font Outline Thickness", sameLine: false);
                    Artist.DrawInputInt($"Font Outline Thickness_{this.Name}", sameLine: true, this.CurrentFontOutlineThickness, 1, 5, thickness => { this.CurrentFontOutlineThickness = thickness; });

                    Artist.DrawLabelPrefix("Select Font Outline Color", sameLine: false);
                    Artist.DrawColorPicker($"Font Outline Color_{this.Name}", sameLine: true, this.CurrentFontOutlineColor, color => { this.CurrentFontOutlineColor = color; });
                }
            }
        }
    }

    /// <summary>
    /// Draw icon configurations.
    /// </summary>
    protected void DrawIconConfigurations()
    {
        Artist.DrawSubTitle("Icon Configurations");
        {
            Artist.DrawLabelPrefix("Enable Icon", sameLine: false);
            Artist.Checkbox($"Enable Icon_{this.Name}", sameLine: true, this.CurrentIconEnabled, enabled => { this.CurrentIconEnabled = enabled; });

            if (this.CurrentIconEnabled)
            {
                Artist.DrawLabelPrefix("Select Icon Size", sameLine: false);
                Artist.DrawInputInt($"Icon Size_{this.Name}", sameLine: true, (int)this.CurrentIconSize, 14, 32, size => { this.CurrentIconSize = size; });

                Artist.DrawSubTitle("Icon Outline Configurations");
                {
                    Artist.DrawLabelPrefix("Enable Icon Outline", sameLine: false);
                    Artist.Checkbox($"Icon Outline_{this.Name}", sameLine: true, this.CurrentIconOutlineEnabled, enabled => { this.CurrentIconOutlineEnabled = enabled; });

                    if (this.CurrentIconOutlineEnabled)
                    {
                        Artist.DrawLabelPrefix("Select Icon Outline Thickness", sameLine: false);
                        Artist.DrawInputInt($"Icon Outline Thickness_{this.Name}", sameLine: true, this.CurrentIconOutlineThickness, 1, 5, thickness => { this.CurrentIconOutlineThickness = thickness; });

                        Artist.DrawLabelPrefix("Select Icon Outline Color", sameLine: false);
                        Artist.DrawColorPicker($"Ocn Outline Color_{this.Name}", sameLine: true, this.CurrentIconOutlineColor, color => { this.CurrentIconOutlineColor = color; });
                    }
                }
            }
        }
    }

    /// <summary>
    /// Draw animation configurations.
    /// </summary>
    protected void DrawAnimationConfigurations()
    {
        Artist.DrawSubTitle("Animation Configuration");
        {
            Artist.DrawLabelPrefix("Reverse Animation", sameLine: false);
            Artist.Checkbox($"Reverse Animation_{this.Name}", sameLine: true, this.CurrentAnimationReversed, enabled => { this.CurrentAnimationReversed = enabled; });
        }
    }
}