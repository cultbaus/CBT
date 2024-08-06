namespace CBT.Interface.Tabs;

using System;
using System.Collections.Generic;
using System.Numerics;
using CBT.FlyText.Configuration;
using ImGuiNET;

/// <summary>
/// TabKind represents the different types of configuration windows.
/// </summary>
[Flags]
public enum TabKind
{
    /// <summary>
    /// Warning for configurations which will overwrite more granular settings.
    /// </summary>
    Warning = 1 << 0,

    /// <summary>
    /// No Warning for configurations which will overwrite more granular settings.
    /// </summary>
    NoWarning = 1 << 1,

    /// <summary>
    /// Tab to configure Group settings.
    /// </summary>
    Group = 1 << 10 | Warning,

    /// <summary>
    /// Tab to configure Category settings.
    /// </summary>
    Category = 1 << 11 | Warning,

    /// <summary>
    /// Tab to configure Kind settings.
    /// </summary>
    Kind = 1 << 12 | NoWarning,

    /// <summary>
    /// Tab to configure Configuration settings.
    /// </summary>
    Settings = 1 << 20 | NoWarning,
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
}