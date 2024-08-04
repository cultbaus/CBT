namespace CBT.FlyText.Types;

/// <summary>
/// DamageKind is the type of damage.
/// </summary>
public enum DamageKind
{
    /// <summary>
    /// All physical damage types.
    /// </summary>
    Physical = 1,

    /// <summary>
    /// All magical damage types.
    /// </summary>
    Magical = 5,

    /// <summary>
    /// All unique or 'dark' damage types.
    /// </summary>
    Unique = 6,

    /// <summary>
    /// All limit break damage types.
    /// </summary>
    LimitBreak = 8,
}