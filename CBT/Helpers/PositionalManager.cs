namespace CBT.Helpers;

using System.Collections.Generic;

/// <summary>
/// State a positional action can be in.
/// </summary>
public enum PositionalState
{
    /// <summary>
    /// Not a positional action.
    /// </summary>
    None,

    /// <summary>
    /// Positional succeeded.
    /// </summary>
    Success,

    /// <summary>
    /// Positional failed.
    /// </summary>
    Failed,
}

/// <summary>
/// Dragoon ability actions.
/// </summary>
public enum Dragoon : uint
{
    /// <summary>
    /// Dragoon positional action.
    /// </summary>
    ChaosThrust = 88,

    /// <summary>
    /// Dragoon positional action.
    /// </summary>
    FangAndClaw = 3554,

    /// <summary>
    /// Dragoon positional action.
    /// </summary>
    WheelingThrust = 3556,

    /// <summary>
    /// Dragoon positional action.
    /// </summary>
    ChaoticSpring = 25772,
}

/// <summary>
/// Monk ability actions.
/// </summary>
public enum Monk : uint
{
    /// <summary>
    /// Monk positional action.
    /// </summary>
    SnapPunch = 56,

    /// <summary>
    /// Monk positional action.
    /// </summary>
    Demolish = 66,

    /// <summary>
    /// Monk positional action.
    /// </summary>
    PouncingCoeurl = 36947,
}

/// <summary>
/// Ninja ability actions.
/// </summary>
public enum Ninja
{
    /// <summary>
    /// Ninja positional action.
    /// </summary>
    AeolianEdge = 2255,

    /// <summary>
    /// Ninja positional action.
    /// </summary>
    TrickAttack = 2258,

    /// <summary>
    /// Ninja positional action.
    /// </summary>
    ArmorCrush = 3563,
}

/// <summary>
/// Reaper ability actions.
/// </summary>
public enum Reaper
{
    /// <summary>
    /// Reaper positional action.
    /// </summary>
    Gibbet = 24382,

    /// <summary>
    /// Reaper positional action.
    /// </summary>
    Gallows = 24383,

    /// <summary>
    /// Reaper positional action.
    /// </summary>
    ExecutionersGibbet = 36970,

    /// <summary>
    /// Reaper positional action.
    /// </summary>
    ExecutionersGallows = 36971,
}

/// <summary>
/// Samurai ability actions.
/// </summary>
public enum Samurai
{
    /// <summary>
    /// Samurai positional action.
    /// </summary>
    Gekko = 7481,

    /// <summary>
    /// Samurai positional action.
    /// </summary>
    Kasha = 7482,
}

/// <summary>
/// Viper ability actions.
/// </summary>
public enum Viper : uint
{
    /// <summary>
    /// Viper positional action.
    /// </summary>
    FlankstingStrike = 34610,

    /// <summary>
    /// Viper positional action.
    /// </summary>
    FlanksbaneFang = 34611,

    /// <summary>
    /// Viper positional action.
    /// </summary>
    HindstringStrike = 34612,

    /// <summary>
    /// Viper positional action.
    /// </summary>
    HindsbaneFang = 34613,

    /// <summary>
    /// Viper positional action.
    /// </summary>
    HuntersCoil = 34621,

    /// <summary>
    /// Viper positional action.
    /// </summary>
    SwiftskinsCoil = 34622,
}

/// <summary>
/// Positional Manager for positional data.
/// </summary>
public class PositionalManager
{
    private static readonly Dictionary<uint, HashSet<int>> PositionalData;

    static PositionalManager() => PositionalData = new()
        {
            { (uint)Dragoon.ChaosThrust, [28, 61] },
            { (uint)Dragoon.FangAndClaw, [22, 28, 58, 66] },
            { (uint)Dragoon.WheelingThrust, [22, 28, 58, 66] },
            { (uint)Dragoon.ChaoticSpring, [22, 28, 58, 66] },
            { (uint)Monk.SnapPunch, [12, 13, 14, 18, 20] },
            { (uint)Monk.Demolish, [14, 15, 17, 18] },
            { (uint)Monk.PouncingCoeurl, [11, 16] },
            { (uint)Ninja.AeolianEdge, [16, 20, 23, 30, 44, 50, 54, 63, 70] },
            { (uint)Ninja.TrickAttack, [25] },
            { (uint)Ninja.ArmorCrush, [21, 30, 54, 65] },
            { (uint)Reaper.Gibbet, [9, 10, 11, 13] },
            { (uint)Reaper.Gallows, [9, 10, 11, 13] },
            { (uint)Reaper.ExecutionersGibbet, [7] },
            { (uint)Reaper.ExecutionersGallows, [7] },
            { (uint)Samurai.Gekko, [23, 31, 33, 61, 70, 72] },
            { (uint)Samurai.Kasha, [23, 31, 33, 61, 70, 72] },
            { (uint)Viper.FlankstingStrike, [48, 54, 60] },
            { (uint)Viper.FlanksbaneFang, [48, 54, 60] },
            { (uint)Viper.HindstringStrike, [48, 54, 60] },
            { (uint)Viper.HindsbaneFang, [48, 54, 60] },
            { (uint)Viper.HuntersCoil, [8] },
            { (uint)Viper.SwiftskinsCoil, [8] },
        };

    /// <summary>
    /// Compare known positional data with received effect data.
    /// </summary>
    /// <remarks>
    /// The positional data isn't exhaustive and also considers valid positionals which fail combo actions to be successful.
    /// </remarks>
    /// <param name="actionID">The ID of the action to check.</param>
    /// <param name="modifier">The `Param2` value on the Effects span for a damage action.</param>
    /// <returns>Positional state indicating the success state.</returns>
    public static PositionalState PositionalSucceeded(int actionID, byte modifier)
        => PositionalData.ContainsKey((uint)actionID)
            ? PositionalData.TryGetValue((uint)actionID, out var successModifiers) && successModifiers.Contains(modifier) ? PositionalState.Success : PositionalState.Failed
            : PositionalState.None;
}