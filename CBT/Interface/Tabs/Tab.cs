namespace CBT.Interface.Tabs;

using System;
using System.Collections.Generic;
using System.Numerics;
using CBT.Attributes;
using CBT.FlyText.Configuration;
using CBT.Types;
using Dalamud.Interface.FontIdentifier;

/// <summary>
/// Tab class.
/// </summary>
public abstract class Tab
{
    /// <summary>
    /// Gets the Name of the window.
    /// </summary>
    public abstract string Name { get; }

    /// <summary>
    /// Gets or sets the current kind.
    /// </summary>
    protected abstract FlyTextKind Current { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether.
    /// </summary>
    protected bool CurrentEnabled
    {
        get => this.GetValue(config => config.Enabled);
        set => this.SetValue((config, val) => config.Enabled = val, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether.
    /// </summary>
    protected bool CurrentEnabledForSelf
    {
        get => this.GetValue(config => config.Filter.Self);
        set => this.SetValue((config, val) => config.Filter.Self = val, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether.
    /// </summary>
    protected bool CurrentEnabledForEnemy
    {
        get => this.GetValue(config => config.Filter.Enemy);
        set => this.SetValue((config, val) => config.Filter.Enemy = val, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether.
    /// </summary>
    protected bool CurrentEnabledForParty
    {
        get => this.GetValue(config => config.Filter.Party);
        set => this.SetValue((config, val) => config.Filter.Party = val, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether.
    /// </summary>
    protected SingleFontSpec CurrentFont
    {
        get => this.GetValue(config => new SingleFontSpec { SizePt = config.Font.Size, FontId = config.Font.FontId });
        set => this.SetValue((config, val) => (config.Font.FontId, config.Font.Size) = (val.FontId, val.SizePt), value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether.
    /// </summary>
    protected Vector4 CurrentFontColor
    {
        get => this.GetValue(config => config.Font.Color);
        set => this.SetValue((config, val) => config.Font.Color = val, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether.
    /// </summary>
    protected float CurrentFontSize
    {
        get => this.GetValue(config => config.Font.Size);
        set => this.SetValue((config, val) => config.Font.Size = val, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether.
    /// </summary>
    protected bool CurrentFontOutlineEnabled
    {
        get => this.GetValue(config => config.Font.Outline.Enabled);
        set => this.SetValue((config, val) => config.Font.Outline.Enabled = val, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether.
    /// </summary>
    protected int CurrentFontOutlineThickness
    {
        get => this.GetValue(config => config.Font.Outline.Size);
        set => this.SetValue((config, val) => config.Font.Outline.Size = val, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether.
    /// </summary>
    protected Vector4 CurrentFontOutlineColor
    {
        get => this.GetValue(config => config.Font.Outline.Color);
        set => this.SetValue((config, val) => config.Font.Outline.Color = val, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether.
    /// </summary>
    protected bool CurrentIconEnabled
    {
        get => this.GetValue(config => config.Icon.Enabled);
        set => this.SetValue((config, val) => config.Icon.Enabled = val, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether.
    /// </summary>
    protected float CurrentIconSize
    {
        get => this.GetValue(config => config.Icon.Size.X);
        set => this.SetValue((config, val) => config.Icon.Size = new Vector2(val, val), value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether.
    /// </summary>
    protected bool CurrentIconOutlineEnabled
    {
        get => this.GetValue(config => config.Icon.Outline.Enabled);
        set => this.SetValue((config, val) => config.Icon.Outline.Enabled = val, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether.
    /// </summary>
    protected int CurrentIconOutlineThickness
    {
        get => this.GetValue(config => config.Icon.Outline.Size);
        set => this.SetValue((config, val) => config.Icon.Outline.Size = val, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether.
    /// </summary>
    protected Vector4 CurrentIconOutlineColor
    {
        get => this.GetValue(config => config.Icon.Outline.Color);
        set => this.SetValue((config, val) => config.Icon.Outline.Color = val, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether.
    /// </summary>
    protected bool CurrentAnimationReversed
    {
        get => this.GetValue(config => config.Animation.Reversed);
        set => this.SetValue((config, val) => config.Animation.Reversed = val, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether.
    /// </summary>
    protected bool CurrentPositionalsEnabled
    {
        get => this.GetValue(config => config.Positionals);
        set => this.SetValue((config, val) => config.Positionals = val, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether.
    /// </summary>
    protected Vector4 CurrentFontColorSuccess
    {
        get => this.GetValue(config => config.Font.ColorSuccess);
        set => this.SetValue((config, val) => config.Font.ColorSuccess = val, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether.
    /// </summary>
    protected Vector4 CurrentFontColorFailed
    {
        get => this.GetValue(config => config.Font.ColorFailed);
        set => this.SetValue((config, val) => config.Font.ColorFailed = val, value);
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
    /// Get a configuration value.
    /// </summary>
    /// <typeparam name="T">The type being configured.</typeparam>
    /// <param name="selector">The means to select the field being retrieved.</param>
    /// <returns>The value.</returns>
    protected T GetValue<T>(Func<FlyTextConfiguration, T> selector)
    {
        return Service.Configuration.FlyTextKinds.TryGetValue(this.Current, out var currentConfig)
            ? selector(currentConfig)
            : selector(Service.Configuration.FlyTextKinds[this.Current]);
    }

    /// <summary>
    /// Set a configuration value.
    /// </summary>
    /// <typeparam name="T">The type being configured.</typeparam>
    /// <param name="setter">The means to set the field.</param>
    /// <param name="value">The value to set.</param>
    protected void SetValue<T>(Action<FlyTextConfiguration, T> setter, T value)
    {
        if (!Service.Configuration.FlyTextKinds.TryGetValue(this.Current, out var currentConfig))
        {
            currentConfig = new FlyTextConfiguration(Service.Configuration.FlyTextKinds[this.Current]);
            Service.Configuration.FlyTextKinds[this.Current] = currentConfig;
        }

        setter(currentConfig, value);
    }

    /// <summary>
    /// Draw configuration options current elements.
    /// </summary>
    /// <param name="allKinds">All kinds to be configured. Can be groups, categories, or kinds.</param>
    protected void DrawCurrentConfigurations(List<FlyTextKind> allKinds)
    {
        GuiArtist.DrawSubTitle($"{this.Name} Configurations");
        {
            GuiArtist.DrawLabelPrefix($"Select {this.Name}", sameLine: false);
            GuiArtist.DrawSelectPicker($"Select_{this.Name}", sameLine: true, this.Current, allKinds, category => { this.Current = category; });

            GuiArtist.DrawLabelPrefix("Enabled", sameLine: false);
            GuiArtist.Checkbox($"Enabled_{this.Name}", sameLine: true, this.CurrentEnabled, enabled => { this.CurrentEnabled = enabled; });

            if (this.CurrentEnabled)
            {
                if (Service.Configuration.FlyTextKinds.TryGetValue(this.Current, out var currentConfig))
                {
                    if (this.Current.ShouldAllow(FlyTextFilter.Self))
                    {
                        GuiArtist.DrawLabelPrefix("Enabled On Self", sameLine: false);
                        GuiArtist.Checkbox($"Enabled On Self_{this.Name}", sameLine: true, this.CurrentEnabledForSelf, enabled => { this.CurrentEnabledForSelf = enabled; });
                    }

                    if (this.Current.ShouldAllow(FlyTextFilter.Enemy))
                    {
                        GuiArtist.DrawLabelPrefix("Enabled On Enemy", sameLine: false);
                        GuiArtist.Checkbox($"Enabled On Enemy_{this.Name}", sameLine: true, this.CurrentEnabledForEnemy, enabled => { this.CurrentEnabledForEnemy = enabled; });
                    }

                    if (this.Current.ShouldAllow(FlyTextFilter.Party))
                    {
                        GuiArtist.DrawLabelPrefix("Enabled On Party", sameLine: false);
                        GuiArtist.Checkbox($"Enabled On Party_{this.Name}", sameLine: true, this.CurrentEnabledForParty, enabled => { this.CurrentEnabledForParty = enabled; });
                    }
                }
            }
        }
    }

    /// <summary>
    /// Draw font configurations.
    /// </summary>
    protected void DrawFontConfigurations()
    {
        GuiArtist.DrawSubTitle("Font Configurations");
        {
            GuiArtist.DrawLabelPrefix("Select Font", sameLine: false);
            GuiArtist.DrawFontPicker($"Font_{this.Name}", sameLine: true, this.CurrentFont, font => { this.CurrentFont = font; });

            GuiArtist.DrawLabelPrefix("Select Font Color", sameLine: false);
            GuiArtist.DrawColorPicker($"Font Color_{this.Name}", sameLine: true, this.CurrentFontColor, color => { this.CurrentFontColor = color; });

            GuiArtist.DrawSubTitle("Font Outline Configurations");
            {
                GuiArtist.DrawLabelPrefix("Enable Font Outline", sameLine: false);
                GuiArtist.Checkbox($"Font Outline_{this.Name}", sameLine: true, this.CurrentFontOutlineEnabled, enabled => { this.CurrentFontOutlineEnabled = enabled; });

                if (this.CurrentFontOutlineEnabled)
                {
                    GuiArtist.DrawLabelPrefix("Select Font Outline Thickness", sameLine: false);
                    GuiArtist.DrawInputInt($"Font Outline Thickness_{this.Name}", sameLine: true, this.CurrentFontOutlineThickness, 1, 5, thickness => { this.CurrentFontOutlineThickness = thickness; });

                    GuiArtist.DrawLabelPrefix("Select Font Outline Color", sameLine: false);
                    GuiArtist.DrawColorPicker($"Font Outline Color_{this.Name}", sameLine: true, this.CurrentFontOutlineColor, color => { this.CurrentFontOutlineColor = color; });
                }
            }
        }
    }

    /// <summary>
    /// Draw icon configurations.
    /// </summary>
    protected void DrawIconConfigurations()
    {
        GuiArtist.DrawSubTitle("Icon Configurations");
        {
            GuiArtist.DrawLabelPrefix("Enable Icon", sameLine: false);
            GuiArtist.Checkbox($"Enable Icon_{this.Name}", sameLine: true, this.CurrentIconEnabled, enabled => { this.CurrentIconEnabled = enabled; });

            if (this.CurrentIconEnabled)
            {
                GuiArtist.DrawLabelPrefix("Select Icon Size", sameLine: false);
                GuiArtist.DrawInputInt($"Icon Size_{this.Name}", sameLine: true, (int)this.CurrentIconSize, 14, 32, size => { this.CurrentIconSize = size; });

                GuiArtist.DrawSubTitle("Icon Outline Configurations");
                {
                    GuiArtist.DrawLabelPrefix("Enable Icon Outline", sameLine: false);
                    GuiArtist.Checkbox($"Icon Outline_{this.Name}", sameLine: true, this.CurrentIconOutlineEnabled, enabled => { this.CurrentIconOutlineEnabled = enabled; });

                    if (this.CurrentIconOutlineEnabled)
                    {
                        GuiArtist.DrawLabelPrefix("Select Icon Outline Thickness", sameLine: false);
                        GuiArtist.DrawInputInt($"Icon Outline Thickness_{this.Name}", sameLine: true, this.CurrentIconOutlineThickness, 1, 5, thickness => { this.CurrentIconOutlineThickness = thickness; });

                        GuiArtist.DrawLabelPrefix("Select Icon Outline Color", sameLine: false);
                        GuiArtist.DrawColorPicker($"Ocn Outline Color_{this.Name}", sameLine: true, this.CurrentIconOutlineColor, color => { this.CurrentIconOutlineColor = color; });
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
        GuiArtist.DrawSubTitle("Animation Configuration");
        {
            GuiArtist.DrawLabelPrefix("Reverse Animation", sameLine: false);
            GuiArtist.Checkbox($"Reverse Animation_{this.Name}", sameLine: true, this.CurrentAnimationReversed, enabled => { this.CurrentAnimationReversed = enabled; });
        }
    }

    /// <summary>
    /// Draw positional color pickers.
    /// </summary>
    protected void DrawPositionalPickers()
    {
        GuiArtist.DrawSubTitle("Positional Color Configuration");
        {
            GuiArtist.DrawLabelPrefix("Enable Positional Recolor", sameLine: false);
            GuiArtist.Checkbox($"Enable Positional Recolor_{this.Name}", sameLine: true, this.CurrentPositionalsEnabled, enabled => { this.CurrentPositionalsEnabled = enabled; });

            if (this.CurrentPositionalsEnabled)
            {
                GuiArtist.DrawLabelPrefix("Select Positional Success Color", sameLine: false);
                GuiArtist.DrawColorPicker($"Font Positional Success Color_{this.Name}", sameLine: true, this.CurrentFontColorSuccess, color => { this.CurrentFontColorSuccess = color; });

                GuiArtist.DrawLabelPrefix("Select Positional Failed Color", sameLine: false);
                GuiArtist.DrawColorPicker($"Font Positional Failed Color_{this.Name}", sameLine: true, this.CurrentFontColorFailed, color => { this.CurrentFontColorFailed = color; });
            }
        }
    }
}