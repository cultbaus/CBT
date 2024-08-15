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
    [FlyTextFilter([FlyTextFilter.Party])]
    [FlyTextCategory(FlyTextCategory.AutoAttack)]
    AutoAttackOrDot = DalamudFlyText.AutoAttackOrDot,

    /// <summary>
    /// Auto attack or dot direct hit.
    /// </summary>
    [FlyTextFilter([FlyTextFilter.Party])]
    [FlyTextCategory(FlyTextCategory.AutoAttack)]
    AutoAttackOrDotDh = DalamudFlyText.AutoAttackOrDotDh,

    /// <summary>
    /// Auto attack or dot crit.
    /// </summary>
    [FlyTextFilter([FlyTextFilter.Party])]
    [FlyTextCategory(FlyTextCategory.AutoAttack)]
    AutoAttackOrDotCrit = DalamudFlyText.AutoAttackOrDotCrit,

    /// <summary>
    /// Auto attack or dot direct-hit-crit.
    /// </summary>
    [FlyTextFilter([FlyTextFilter.Party])]
    [FlyTextCategory(FlyTextCategory.AutoAttack)]
    AutoAttackOrDotCritDh = DalamudFlyText.AutoAttackOrDotCritDh,

    /// <summary>
    /// Ability damage.
    /// </summary>
    [FlyTextFilter([FlyTextFilter.Party])]
    [FlyTextCategory(FlyTextCategory.AbilityDamage)]
    Damage = DalamudFlyText.Damage,

    /// <summary>
    /// Ability damage direct hit.
    /// </summary>
    [FlyTextFilter([FlyTextFilter.Party])]
    [FlyTextCategory(FlyTextCategory.AbilityDamage)]
    DamageDh = DalamudFlyText.DamageDh,

    /// <summary>
    /// Ability damage crit.
    /// </summary>
    [FlyTextFilter([FlyTextFilter.Party])]
    [FlyTextCategory(FlyTextCategory.AbilityDamage)]
    DamageCrit = DalamudFlyText.DamageCrit,

    /// <summary>
    /// Ability damage direct-hit-crit.
    /// </summary>
    [FlyTextFilter([FlyTextFilter.Party])]
    [FlyTextCategory(FlyTextCategory.AbilityDamage)]
    DamageCritDh = DalamudFlyText.DamageCritDh,

    /// <summary>
    /// Miss, but sometimes dodge.
    /// </summary>
    [FlyTextFilter([FlyTextFilter.Party])]
    [FlyTextCategory(FlyTextCategory.Miss)]
    Miss = DalamudFlyText.Miss,

    /// <summary>
    /// Named miss.
    /// </summary>
    [FlyTextFilter([FlyTextFilter.Party])]
    [FlyTextCategory(FlyTextCategory.Miss)]
    NamedMiss = DalamudFlyText.NamedMiss,

    /// <summary>
    /// Dodge.
    /// </summary>
    [FlyTextFilter([FlyTextFilter.Party])]
    [FlyTextCategory(FlyTextCategory.Miss)]
    Dodge = DalamudFlyText.Dodge,

    /// <summary>
    /// Dodge.
    /// </summary>
    [FlyTextFilter([FlyTextFilter.Party])]
    [FlyTextCategory(FlyTextCategory.Miss)]
    NamedDodge = DalamudFlyText.NamedDodge,

    /// <summary>
    /// Buff.
    /// </summary>
    [FlyTextFilter([FlyTextFilter.Enemy])]
    [FlyTextCategory(FlyTextCategory.Buff)]
    Buff = DalamudFlyText.Buff,

    /// <summary>
    /// Debuff.
    /// </summary>
    [FlyTextFilter([FlyTextFilter.Party])]
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
    [FlyTextFilter([FlyTextFilter.Enemy])]
    [FlyTextCategory(FlyTextCategory.AbilityHealing)]
    Healing = DalamudFlyText.Healing,

    /// <summary>
    /// MpRegen ticks.
    /// </summary>
    [FlyTextFilter([FlyTextFilter.Enemy])]
    [FlyTextCategory(FlyTextCategory.AbilityHealing)]
    MpRegen = DalamudFlyText.MpRegen,

    /*
    * NamedTp2,
    * EpRegen,
    * CpRegen,
    * GpRegen,
    */

    /// <summary>
    /// None.
    /// </summary>
    [FlyTextFilter([FlyTextFilter.Party, FlyTextFilter.Enemy, FlyTextFilter.Self])]
    [FlyTextCategory(FlyTextCategory.CategoryNone)]
    None = DalamudFlyText.None,

    /// <summary>
    /// Invulnerable.
    /// </summary>
    [FlyTextFilter([FlyTextFilter.Party, FlyTextFilter.Self])]
    [FlyTextCategory(FlyTextCategory.Miss)]
    Invulnerable = DalamudFlyText.Invulnerable,

    /*
    * Interrupted,
    * CraftingProgress,
    * CraftingQuality,
    * CraftingQualityCrit,
    */

    /// <summary>
    /// Healing crit.
    /// </summary>
    [FlyTextFilter([FlyTextFilter.Enemy])]
    [FlyTextCategory(FlyTextCategory.AbilityHealing)]
    HealingCrit = DalamudFlyText.HealingCrit,

    /// <summary>
    /// Debuff "has no effect".
    /// </summary>
    [FlyTextFilter([FlyTextFilter.Party])]
    [FlyTextCategory(FlyTextCategory.Miss)]
    DebuffNoEffect = DalamudFlyText.DebuffNoEffect,

    /// <summary>
    /// Buff with faded text.
    /// </summary>
    [FlyTextFilter([FlyTextFilter.Enemy])]
    [FlyTextCategory(FlyTextCategory.Buff)]
    BuffFading = DalamudFlyText.BuffFading,

    /// <summary>
    /// Debuff with faded text.
    /// </summary>
    [FlyTextFilter([FlyTextFilter.Party])]
    [FlyTextCategory(FlyTextCategory.Debuff)]
    DebuffFading = DalamudFlyText.DebuffFading,

    /// <summary>
    /// Named. Unknown.
    /// </summary>
    [FlyTextFilter([FlyTextFilter.Party])]
    [FlyTextCategory(FlyTextCategory.Debuff)]
    Named = DalamudFlyText.Named,

    /// <summary>
    /// Debuff was resisted.
    /// </summary>
    [FlyTextFilter([FlyTextFilter.Party])]
    [FlyTextCategory(FlyTextCategory.Miss)]
    DebuffResisted = DalamudFlyText.DebuffResisted,

    /// <summary>
    /// Target is incapacitated.
    /// </summary>
    [FlyTextFilter([FlyTextFilter.None])]
    [FlyTextCategory(FlyTextCategory.CC)]
    Incapacitated = DalamudFlyText.Incapacitated,

    /// <summary>
    /// Ability is fully resisted.
    /// </summary>
    [FlyTextFilter([FlyTextFilter.Party])]
    [FlyTextCategory(FlyTextCategory.Miss)]
    FullyResisted = DalamudFlyText.FullyResisted,

    /// <summary>
    /// Ability has no effect.
    /// </summary>
    [FlyTextFilter([FlyTextFilter.Party])]
    [FlyTextCategory(FlyTextCategory.Miss)]
    HasNoEffect = DalamudFlyText.HasNoEffect,

    /// <summary>
    /// HP Drain, bloodbath.
    /// </summary>
    [FlyTextFilter([FlyTextFilter.Party])]
    [FlyTextCategory(FlyTextCategory.AbilityHealing)]
    HpDrain = DalamudFlyText.HpDrain,

    /// <summary>
    /// Debuff immued due to invuln.
    /// </summary>
    [FlyTextFilter([FlyTextFilter.Party])]
    [FlyTextCategory(FlyTextCategory.Miss)]
    DebuffInvulnerable = DalamudFlyText.DebuffInvulnerable,

    /// <summary>
    /// Resisted.
    /// </summary>
    [FlyTextFilter([FlyTextFilter.Party])]
    [FlyTextCategory(FlyTextCategory.Miss)]
    Resist = DalamudFlyText.Resist,

    /// <summary>
    /// Looted Item.
    /// </summary>
    [FlyTextFilter([FlyTextFilter.Party, FlyTextFilter.Enemy])]
    [FlyTextCategory(FlyTextCategory.Other)]
    LootedItem = DalamudFlyText.LootedItem,

    /// <summary>
    /// Collectible.
    /// </summary>
    [FlyTextFilter([FlyTextFilter.Party, FlyTextFilter.Enemy])]
    [FlyTextCategory(FlyTextCategory.Other)]
    Collectability = DalamudFlyText.Collectability,

    /// <summary>
    /// Crit collectible.
    /// </summary>
    [FlyTextFilter([FlyTextFilter.Party, FlyTextFilter.Enemy])]
    [FlyTextCategory(FlyTextCategory.Other)]
    CollectabilityCrit = DalamudFlyText.CollectabilityCrit,

    /// <summary>
    /// Reflect.
    /// </summary>
    [FlyTextFilter([FlyTextFilter.Party])]
    [FlyTextCategory(FlyTextCategory.Miss)]
    Reflect = DalamudFlyText.Reflect,

    /// <summary>
    /// Reflected.
    /// </summary>
    [FlyTextFilter([FlyTextFilter.Party])]
    [FlyTextCategory(FlyTextCategory.Miss)]
    Reflected = DalamudFlyText.Reflected,

    /// <summary>
    /// Crafting quality DH.
    /// </summary>
    [FlyTextFilter([FlyTextFilter.Party, FlyTextFilter.Enemy])]
    [FlyTextCategory(FlyTextCategory.Other)]
    CraftingQualityDh = DalamudFlyText.CraftingQualityDh,

    /// <summary>
    /// Crafting Quality crit DH.
    /// </summary>
    [FlyTextFilter([FlyTextFilter.Party, FlyTextFilter.Enemy])]
    [FlyTextCategory(FlyTextCategory.Other)]
    CraftingQualityCritDh = DalamudFlyText.CraftingQualityCritDh,
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
    /// Get the constant filter for an event.
    /// </summary>
    /// <param name="kind">FlyTextKidn enum.</param>
    /// <returns>The filter for the kind.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Throws if the kind has no filter.</exception>
    public static FlyTextFilter[] GetFilter(this FlyTextKind kind)
    {
        var attr = typeof(FlyTextKind)
            .GetMember(kind.ToString())[0]
            .GetCustomAttributes(typeof(FlyTextFilterAttribute), false);

        return attr.Length > 0
            ? ((FlyTextFilterAttribute)attr[0]).Filter
            : throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
    }

    /// <summary>
    /// Whether the event should filter against the type.
    /// </summary>
    /// <param name="kind">Kind to filter.</param>
    /// <param name="filter">Filter to compare.</param>
    /// <returns>Bool.</returns>
    public static bool ShouldFilter(this FlyTextKind kind, FlyTextFilter filter)
        => kind.GetFilter().Length > 0 && kind.GetFilter().Contains(filter);

    /// <summary>
    /// Opposite of ShouldFilter.
    /// </summary>
    /// <param name="kind">Kind to allow.</param>
    /// <param name="filter">Filter to compare.</param>
    /// <returns>Bool.</returns>
    public static bool ShouldAllow(this FlyTextKind kind, FlyTextFilter filter)
        => !kind.ShouldFilter(filter);

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
        => kind switch
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
            FlyTextKind.Invulnerable => "Invulnerable",
            FlyTextKind.Resist => "Resisted",
            _ => string.Empty,
        };

    /// <summary>
    /// Predicate for filtering unused kinds.
    /// </summary>
    /// <param name="kind">The kind to compare.</param>
    /// <returns>A bool if the value should filter.</returns>
    public static bool UnusedKindPredicate(FlyTextKind kind)
        => kind != FlyTextKind.None;
}