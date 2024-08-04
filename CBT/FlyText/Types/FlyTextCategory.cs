namespace CBT.FlyText.Types;

using System;

/// <summary>
/// FlyTextCategory is a collect of Groups and Categories which organize a subset of <see cref="FlyTextKind"/>s.
/// </summary>
[Flags]
public enum FlyTextCategory
{
    // Groups

    /// <summary>
    /// Combat group.
    /// </summary>
    Combat = 1 << 0,

    /// <summary>
    /// Non-combat group.
    /// </summary>
    NonCombat = 1 << 1,

    /// <summary>
    /// Combat abilities which explicitly deal damage of a kind.
    /// </summary>
    DamageDealer = 1 << 2,

    // Categories

    /// <summary>
    /// AutoAttack category. Member of the Combat group.
    /// </summary>
    AutoAttack = 1 << 10 | Combat | DamageDealer,

    /// <summary>
    /// AbilityDamage category. Member of the Combat group.
    /// </summary>
    AbilityDamage = 1 << 11 | Combat | DamageDealer,

    /// <summary>
    /// AbilityHealing category. Member of the Combat group.
    /// </summary>
    AbilityHealing = 1 << 12 | Combat,

    /// <summary>
    /// Miss category. Member of the Combat group.
    /// </summary>
    Miss = 1 << 13 | Combat,

    /// <summary>
    /// Buff category. Member of the Combat group.
    /// </summary>
    Buff = 1 << 14 | Combat,

    /// <summary>
    /// Debuff category. Member of the Combat group.
    /// </summary>
    Debuff = 1 << 15 | Combat,

    /// <summary>
    /// CC category. Member of the Combat group.
    /// </summary>
    CC = 1 << 16 | Combat,
}