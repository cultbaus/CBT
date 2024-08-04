namespace CBT.Helpers;

using System.Collections.Generic;
using Lumina.Excel;
using Lumina.Excel.GeneratedSheets;

/// <summary>
/// ActionManager accesses the Dalamud ActionManager.
/// </summary>
public unsafe class AbilityManager
{
    private static readonly ExcelSheet<Action>? LuminaActionSheet = Service.DataManager.GetExcelSheet<Action>();
    private static readonly ExcelSheet<Status>? LuminaStatusSheet = Service.DataManager.GetExcelSheet<Status>();

    private readonly Dictionary<int, Action?> actionCache = new Dictionary<int, Action?>();
    private readonly Dictionary<int, Status?> statusCache = new Dictionary<int, Status?>();

    /// <summary>
    /// Get an Icon ID for the given actionID.
    /// </summary>
    /// <param name="actionID">Action ID.</param>
    /// <returns>Icon ID.</returns>
    public ushort GetIconForAction(int actionID)
         => this.actionCache.TryGetValue(actionID, out var action) ? action?.Icon ?? 0 : this.GetActionRow(actionID)?.Icon ?? 0;

    /// <summary>
    /// Get the Ability Name for the given actionID.
    /// </summary>
    /// <param name="actionID">Action ID.</param>
    /// <returns>Ability name.</returns>
    public string GetNameForAction(int actionID)
         => this.actionCache.TryGetValue(actionID, out var action) ? action?.Name ?? string.Empty : this.GetActionRow(actionID)?.Name ?? string.Empty;

    /// <summary>
    /// Get an Icon ID for the given actionID.
    /// </summary>
    /// <param name="value1">Action ID.</param>
    /// <returns>Icon ID.</returns>
    public ushort GetIconForStatus(int value1)
         => this.actionCache.TryGetValue(value1, out var status) ? status?.Icon ?? 0 : (ushort)(this.GetStatusRow(value1)?.Icon ?? 0);

    /// <summary>
    /// Get the Ability Name for the given actionID.
    /// </summary>
    /// <param name="value1">Action ID.</param>
    /// <returns>Ability name.</returns>
    public string GetNameForStatus(int value1)
         => this.actionCache.TryGetValue(value1, out var status) ? status?.Name ?? string.Empty : this.GetStatusRow(value1)?.Name ?? string.Empty;

    private Action? GetActionRow(int actionID)
    {
        var row = LuminaActionSheet?.GetRow((uint)actionID);
        if (row != null)
        {
            this.actionCache[actionID] = row;
        }

        return this.actionCache[actionID];
    }

    private Status? GetStatusRow(int value1)
    {
        var row = LuminaStatusSheet?.GetRow((uint)value1);
        if (row != null)
        {
            this.statusCache[value1] = row;
        }

        return this.statusCache[value1];
    }
}