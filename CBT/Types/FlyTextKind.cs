namespace CBT.Types;

using System;
using System.Collections.Generic;
using System.Linq;
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
    */

    /// <summary>
    /// None.
    /// </summary>
    [FlyTextCategory(FlyTextCategory.CategoryNone)]
    None = DalamudFlyText.None,

    /*
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

/// <summary>
/// FlyTextKinds implements methods on the FlyTextKind enum.
/// </summary>
public static class FlyTextKindExtension
{
    /// <summary>
    /// Gets the category for a given FlyTextKind.
    /// </summary>
    /// <param name="kind">FlyTextKind enum.</param>
    /// <returns>Category for which the kind is a member.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Throws if the kind has no category.</exception>
    public static FlyTextCategory GetCategory(this FlyTextKind kind)
    {
        var attr = typeof(FlyTextKind)
            .GetMember(kind.ToString())[0]
            .GetCustomAttributes(typeof(FlyTextCategoryAttribute), false);

        return attr.Length > 0
            ? ((FlyTextCategoryAttribute)attr[0]).Category
            : throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
    }

    /// <summary>
    /// Checks if a kind is in a category.
    /// </summary>
    /// <param name="kind">FlyTextKind to look up.</param>
    /// <param name="category">FlyTextCategory to check if kind is a member of.</param>
    /// <returns>True if the kind is a member of that category.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Throws if the category is a group.</exception>
    public static bool InCategory(this FlyTextKind kind, FlyTextCategory category)
        => category.IsCategory()
            ? GetCategory(kind) == category
            : throw new ArgumentOutOfRangeException(nameof(category), category, null);

    /// <summary>
    /// Checks if a kind is in a group.
    /// </summary>
    /// <param name="kind">FlyTextKind to look up.</param>
    /// <param name="group">FlyTextCategory to check if kind is a member of.</param>
    /// <returns>True if the kind is a member of that group.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Throws if the group is a category.</exception>
    public static bool InGroup(this FlyTextKind kind, FlyTextCategory group)
        => group.IsGroup()
            ? GetCategory(kind).HasFlag(group)
            : throw new ArgumentOutOfRangeException(nameof(group), group, null);

    /// <summary>
    /// Checks if a kind is a status.
    /// </summary>
    /// <param name="kind">FlyTextKind.</param>
    /// <returns>Bool indicating if the kind is a status effect.</returns>
    public static bool IsStatus(this FlyTextKind kind)
        => kind.InCategory(FlyTextCategory.Buff) || kind.InCategory(FlyTextCategory.Debuff);

    /// <summary>
    /// Checks if a kind is a message-only.
    /// </summary>
    /// <param name="kind">FlyTextKind.</param>
    /// <returns>Bool indicating if the kind is a message-only kind.</returns>
    public static bool IsMessage(this FlyTextKind kind)
        => kind.InCategory(FlyTextCategory.Miss) || kind.InCategory(FlyTextCategory.CC);

    /// <summary>
    /// Checks if a kind is a spell.
    /// </summary>
    /// <param name="kind">FlyTextKind.</param>
    /// <returns>Bool indicating if the kind is a spell/ability.</returns>
    public static bool IsSpell(this FlyTextKind kind)
        => !IsStatus(kind);

    /// <summary>
    /// Gets all kinds.
    /// </summary>
    /// <returns>All kinds.</returns>
    public static IEnumerable<FlyTextKind> GetAll()
        => Enum.GetValues<FlyTextKind>()
            .Cast<FlyTextKind>();

    /// <summary>
    /// Pretty print a value.
    /// </summary>
    /// <param name="kind">Kind to print.</param>
    /// <returns>A pretty string.</returns>
    public static string Pretty(this FlyTextKind kind)
    {
        return kind switch
        {
            FlyTextKind.Miss => "Miss",
            FlyTextKind.NamedMiss => "Miss",
            FlyTextKind.Dodge => "Dodge",
            FlyTextKind.NamedDodge => "Dodge",
            FlyTextKind.DebuffNoEffect => "No Effect",
            FlyTextKind.DebuffResisted => "Resisted",
            FlyTextKind.Incapacitated => "Incapacitated",
            FlyTextKind.FullyResisted => "Resisted",
            FlyTextKind.HasNoEffect => "No Effect",
            FlyTextKind.DebuffInvulnerable => "Invulnerable",
            FlyTextKind.Resist => "Resisted",
            _ => string.Empty,
        };
    }
}