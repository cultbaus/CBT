namespace CBT.FlyText.Types;

using CBT.Attributes;
using DalamudFlyText = Dalamud.Game.Gui.FlyText.FlyTextKind;

/// <summary>
/// FlyTextKind is the CBT wrapped enum of the Dalamud FlyTextKind enum.
/// </summary>
public enum FlyTextKind
{
    /// <summary>
    /// Auto attack or dot.
    /// </summary>
    [FlyTextCategory(FlyTextCategory.AutoAttack)]
    AutoAttackOrDot = DalamudFlyText.AutoAttackOrDot,

    /// <summary>
    /// Auto attack or dot direct hit.
    /// </summary>
    [FlyTextCategory(FlyTextCategory.AutoAttack)]
    AutoAttackOrDotDh = DalamudFlyText.AutoAttackOrDotDh,

    /// <summary>
    /// Auto attack or dot crit.
    /// </summary>
    [FlyTextCategory(FlyTextCategory.AutoAttack)]
    AutoAttackOrDotCrit = DalamudFlyText.AutoAttackOrDotCrit,

    /// <summary>
    /// Auto attack or dot direct-hit-crit.
    /// </summary>
    [FlyTextCategory(FlyTextCategory.AutoAttack)]
    AutoAttackOrDotCritDh = DalamudFlyText.AutoAttackOrDotCritDh,

    /// <summary>
    /// Ability damage.
    /// </summary>
    [FlyTextCategory(FlyTextCategory.AbilityDamage)]
    Damage = DalamudFlyText.Damage,

    /// <summary>
    /// Ability damage direct hit.
    /// </summary>
    [FlyTextCategory(FlyTextCategory.AbilityDamage)]
    DamageDh = DalamudFlyText.DamageDh,

    /// <summary>
    /// Ability damage crit.
    /// </summary>
    [FlyTextCategory(FlyTextCategory.AbilityDamage)]
    DamageCrit = DalamudFlyText.DamageCrit,

    /// <summary>
    /// Ability damage direct-hit-crit.
    /// </summary>
    [FlyTextCategory(FlyTextCategory.AbilityDamage)]
    DamageCritDh = DalamudFlyText.DamageCritDh,

    /// <summary>
    /// Miss, but sometimes dodge.
    /// </summary>
    [FlyTextCategory(FlyTextCategory.Miss)]
    Miss = DalamudFlyText.Miss,

    /// <summary>
    /// Named miss.
    /// </summary>
    [FlyTextCategory(FlyTextCategory.Miss)]
    NamedMiss = DalamudFlyText.NamedMiss,

    /// <summary>
    /// Dodge.
    /// </summary>
    [FlyTextCategory(FlyTextCategory.Miss)]
    Dodge = DalamudFlyText.Dodge,

    /// <summary>
    /// Dodge.
    /// </summary>
    [FlyTextCategory(FlyTextCategory.Miss)]
    NamedDodge = DalamudFlyText.NamedDodge,

    /// <summary>
    /// Buff.
    /// </summary>
    [FlyTextCategory(FlyTextCategory.Buff)]
    Buff = DalamudFlyText.Buff,

    /// <summary>
    /// Debuff.
    /// </summary>
    [FlyTextCategory(FlyTextCategory.Debuff)]
    Debuff = DalamudFlyText.Debuff,

    /*
    * Exp,
    * IslandExp,
    * MpDrain,
    * NamedTp,
    */

    /// <summary>
    /// Healing.
    /// </summary>
    [FlyTextCategory(FlyTextCategory.AbilityHealing)]
    Healing = DalamudFlyText.Healing,

    /*
    * MpRegen,
    * NamedTp2,
    * EpRegen,
    * CpRegen,
    * GpRegen,
    * None,
    * Invulnerable,
    * Interrupted,
    * CraftingProgress,
    * CraftingQuality,
    * CraftingQualityCrit,
    */

    /// <summary>
    /// Healing crit.
    /// </summary>
    [FlyTextCategory(FlyTextCategory.AbilityHealing)]
    HealingCrit = DalamudFlyText.HealingCrit,

    /// <summary>
    /// Debuff "has no effect".
    /// </summary>
    [FlyTextCategory(FlyTextCategory.Miss)]
    DebuffNoEffect = DalamudFlyText.DebuffNoEffect,

    /// <summary>
    /// Buff with faded text.
    /// </summary>
    [FlyTextCategory(FlyTextCategory.Buff)]
    BuffFading = DalamudFlyText.BuffFading,

    /// <summary>
    /// Debuff with faded text.
    /// </summary>
    [FlyTextCategory(FlyTextCategory.Debuff)]
    DebuffFading = DalamudFlyText.DebuffFading,

    /*
    * Named
    */

    /// <summary>
    /// Debuff was resisted.
    /// </summary>
    [FlyTextCategory(FlyTextCategory.Miss)]
    DebuffResisted = DalamudFlyText.DebuffResisted,

    /// <summary>
    /// Target is incapacitated.
    /// </summary>
    [FlyTextCategory(FlyTextCategory.CC)]
    Incapacitated = DalamudFlyText.Incapacitated,

    /// <summary>
    /// Ability is fully resisted.
    /// </summary>
    [FlyTextCategory(FlyTextCategory.Miss)]
    FullyResisted = DalamudFlyText.FullyResisted,

    /// <summary>
    /// Ability has no effect.
    /// </summary>
    [FlyTextCategory(FlyTextCategory.Miss)]
    HasNoEffect = DalamudFlyText.HasNoEffect,

    /// <summary>
    /// HP Drain, bloodbath.
    /// </summary>
    [FlyTextCategory(FlyTextCategory.AbilityHealing)]
    HpDrain = DalamudFlyText.HpDrain,

    /// <summary>
    /// Debuff immued due to invuln.
    /// </summary>
    [FlyTextCategory(FlyTextCategory.Miss)]
    DebuffInvulnerable = DalamudFlyText.DebuffInvulnerable,

    /// <summary>
    /// Resisted.
    /// </summary>
    [FlyTextCategory(FlyTextCategory.Miss)]
    Resist = DalamudFlyText.Resist,

    /*
    * LootedItem,
    * Collectability,
    * CollectabilityCrit,
    * Reflect,
    * Reflected,
    * CraftingQualityDh,
    * CraftingQualityCritDh
    */
}