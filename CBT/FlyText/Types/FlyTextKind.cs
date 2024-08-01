namespace CBT.FlyText.Types;

using CBT.Attributes;
using DalamudFlyText = Dalamud.Game.Gui.FlyText;

/// <summary>
/// FlyTextKind is the CBT wrapped enum of the Dalamud FlyTextKind enum.
/// </summary>
public enum FlyTextKind
{
    /// <summary>
    /// Auto attack or dot.
    /// </summary>
    [FlyTextCategory(FlyTextCategory.AutoAttack)]
    AutoAttackOrDot = DalamudFlyText.FlyTextKind.AutoAttackOrDot,

    /// <summary>
    /// Auto attack or dot direct hit.
    /// </summary>
    [FlyTextCategory(FlyTextCategory.AutoAttack)]
    AutoAttackOrDotDh = DalamudFlyText.FlyTextKind.AutoAttackOrDotDh,

    /// <summary>
    /// Auto attack or dot crit.
    /// </summary>
    [FlyTextCategory(FlyTextCategory.AutoAttack)]
    AutoAttackOrDotCrit = DalamudFlyText.FlyTextKind.AutoAttackOrDotCrit,

    /// <summary>
    /// Auto attack or dot direct-hit-crit.
    /// </summary>
    [FlyTextCategory(FlyTextCategory.AutoAttack)]
    AutoAttackOrDotCritDh = DalamudFlyText.FlyTextKind.AutoAttackOrDotCritDh,

    /// <summary>
    /// Ability damage.
    /// </summary>
    [FlyTextCategory(FlyTextCategory.AbilityDamage)]
    Damage = DalamudFlyText.FlyTextKind.Damage,

    /// <summary>
    /// Ability damage direct hit.
    /// </summary>
    [FlyTextCategory(FlyTextCategory.AbilityDamage)]
    DamageDh = DalamudFlyText.FlyTextKind.DamageDh,

    /// <summary>
    /// Ability damage crit.
    /// </summary>
    [FlyTextCategory(FlyTextCategory.AbilityDamage)]
    DamageCrit = DalamudFlyText.FlyTextKind.DamageCrit,

    /// <summary>
    /// Ability damage direct-hit-crit.
    /// </summary>
    [FlyTextCategory(FlyTextCategory.AbilityDamage)]
    DamageCritDh = DalamudFlyText.FlyTextKind.DamageCritDh,

    /// <summary>
    /// Miss, but sometimes dodge.
    /// </summary>
    [FlyTextCategory(FlyTextCategory.Miss)]
    Miss = DalamudFlyText.FlyTextKind.Miss,

    /// <summary>
    /// Named miss.
    /// </summary>
    [FlyTextCategory(FlyTextCategory.Miss)]
    NamedMiss = DalamudFlyText.FlyTextKind.NamedMiss,

    /// <summary>
    /// Dodge.
    /// </summary>
    [FlyTextCategory(FlyTextCategory.Miss)]
    Dodge = DalamudFlyText.FlyTextKind.Dodge,

    /// <summary>
    /// Dodge.
    /// </summary>
    [FlyTextCategory(FlyTextCategory.Miss)]
    NamedDodge = DalamudFlyText.FlyTextKind.NamedDodge,

    /// <summary>
    /// Buff.
    /// </summary>
    [FlyTextCategory(FlyTextCategory.Buff)]
    Buff = DalamudFlyText.FlyTextKind.Buff,

    /// <summary>
    /// Debuff.
    /// </summary>
    [FlyTextCategory(FlyTextCategory.Debuff)]
    Debuff = DalamudFlyText.FlyTextKind.Debuff,

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
    Healing = DalamudFlyText.FlyTextKind.Healing,

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
    HealingCrit = DalamudFlyText.FlyTextKind.HealingCrit,

    /// <summary>
    /// Debuff "has no effect".
    /// </summary>
    [FlyTextCategory(FlyTextCategory.Miss)]
    DebuffNoEffect = DalamudFlyText.FlyTextKind.DebuffNoEffect,

    /// <summary>
    /// Buff with faded text.
    /// </summary>
    [FlyTextCategory(FlyTextCategory.Buff)]
    BuffFading = DalamudFlyText.FlyTextKind.BuffFading,

    /// <summary>
    /// Debuff with faded text.
    /// </summary>
    [FlyTextCategory(FlyTextCategory.Debuff)]
    DebuffFading = DalamudFlyText.FlyTextKind.DebuffFading,

    /*
    * Named
    */

    /// <summary>
    /// Debuff was resisted.
    /// </summary>
    [FlyTextCategory(FlyTextCategory.Miss)]
    DebuffResisted = DalamudFlyText.FlyTextKind.DebuffResisted,

    /// <summary>
    /// Target is incapacitated.
    /// </summary>
    [FlyTextCategory(FlyTextCategory.CC)]
    Incapacitated = DalamudFlyText.FlyTextKind.Incapacitated,

    /// <summary>
    /// Ability is fully resisted.
    /// </summary>
    [FlyTextCategory(FlyTextCategory.Miss)]
    FullyResisted = DalamudFlyText.FlyTextKind.FullyResisted,

    /// <summary>
    /// Ability has no effect.
    /// </summary>
    [FlyTextCategory(FlyTextCategory.Miss)]
    HasNoEffect = DalamudFlyText.FlyTextKind.HasNoEffect,

    /// <summary>
    /// HP Drain, bloodbath.
    /// </summary>
    [FlyTextCategory(FlyTextCategory.AbilityHealing)]
    HpDrain = DalamudFlyText.FlyTextKind.HpDrain,

    /// <summary>
    /// Debuff immued due to invuln.
    /// </summary>
    [FlyTextCategory(FlyTextCategory.Miss)]
    DebuffInvulnerable = DalamudFlyText.FlyTextKind.DebuffInvulnerable,

    /// <summary>
    /// Resisted.
    /// </summary>
    [FlyTextCategory(FlyTextCategory.Miss)]
    Resist = DalamudFlyText.FlyTextKind.Resist,

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