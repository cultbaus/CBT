namespace CBT.Interface.Tabs;

using System;

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
/// Tab is the abstract class which represents a configuration tab.
/// </summary>
public abstract class Tab : IDisposable
{
    /// <summary>
    /// Gets the Name of the window.
    /// </summary>
    public abstract string Name { get; }

    /// <summary>
    /// Gets the Kind of the window.
    /// </summary>
    public abstract TabKind Kind { get; }

    /// <inheritdoc/>
    public virtual void Dispose()
    {
    }

    /// <summary>
    /// Draws the Window associated with a Tab.
    /// </summary>
    public abstract void Draw();

    /// <summary>
    /// What to do on Close event.
    /// </summary>
    public abstract void OnClose();
}