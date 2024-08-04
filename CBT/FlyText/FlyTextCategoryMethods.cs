namespace CBT.FlyText;

using System;
using System.Collections.Generic;
using System.Linq;
using CBT.FlyText.Types;

/// <summary>
/// FlyTextCategory methods.
/// </summary>
public static class FlyTextCategoryMethods
{
    /// <summary>
    /// Gets kinds for a given category.
    /// </summary>
    /// <param name="category">FlyTextCategory to fetch kinds for.</param>
    /// <returns>A list of all kinds within that category.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Throws if the category is a group.</exception>
    public static List<FlyTextKind> GetKindsForCategory(this FlyTextCategory category)
        => IsCategory(category)
            ? Enum.GetValues<FlyTextKind>()
                .Cast<FlyTextKind>()
                .Where(value => value.GetCategory() == category)
                .ToList()
            : throw new ArgumentOutOfRangeException(nameof(category), category, null);

    /// <summary>
    /// Gets kinds for a given group.
    /// </summary>
    /// <param name="group">FlyTextCategory group to fetch kinds for.</param>
    /// <returns>A list of all kinds within that group.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Throws if the group is a category.</exception>
    public static List<FlyTextKind> GetKindsForGroup(this FlyTextCategory group)
        => IsGroup(group)
            ? Enum.GetValues<FlyTextKind>()
                .Cast<FlyTextKind>()
                .Where(value => value.GetCategory().HasFlag(group))
                .ToList()
            : throw new ArgumentOutOfRangeException(nameof(group), group, null);

    /// <summary>
    /// Gets categories for a given group.
    /// </summary>
    /// <param name="group">FlyTextCategory group to fetch categories for.</param>
    /// <returns>A list of all categories within that group.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Throws if the group is a category.</exception>
    public static List<FlyTextCategory> GetCategoriesForGroup(this FlyTextCategory group)
        => IsGroup(group)
        ? Enum.GetValues<FlyTextCategory>()
            .Cast<FlyTextCategory>()
            .Where(value => value.HasFlag(group))
            .ToList()
        : throw new ArgumentOutOfRangeException(nameof(group), group, null);

    /// <summary>
    /// Determines if a FlyTextCategory is a Group sub-type.
    /// </summary>
    /// <param name="value">FlyTextCategory to check.</param>
    /// <returns>True if the category is a group sub-type.</returns>
    public static bool IsGroup(this FlyTextCategory value)
        => value == FlyTextCategory.Combat || value == FlyTextCategory.NonCombat || value == FlyTextCategory.DamageDealer;

    /// <summary>
    /// Determines if a FlyTextCategory is a Category sub-type.
    /// </summary>
    /// <param name="value">FlyTextCategory to check.</param>
    /// <returns>True if the category is a category sub-type.</returns>
    public static bool IsCategory(this FlyTextCategory value)
        => !IsGroup(value);

    /// <summary>
    /// Iterates the kinds of a category or group and performs an action.
    /// </summary>
    /// <param name="category">The category or group to iterate.</param>
    /// <param name="action">The action to perform.</param>
    public static void ForEach(this FlyTextCategory category, Action<FlyTextKind> action)
    {
        Enum.GetValues<FlyTextKind>()
           .Cast<FlyTextKind>()
           .Where(kind => IsCategory(category) ? kind.InCategory(category) : kind.InGroup(category))
           .ToList()
           .ForEach(action);
    }

    /// <summary>
    /// Gets the pretty print name for a category.
    /// </summary>
    /// <param name="category">FlyTextCategory.</param>
    /// <returns>Pretty printed name.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Throws if argument is out of range.</exception>
    public static string Pretty(this FlyTextCategory category)
    {
        return category switch
        {
            FlyTextCategory.Combat => "Combat",
            FlyTextCategory.NonCombat => "NonCombat",
            FlyTextCategory.AutoAttack => "Auto Attacks",
            FlyTextCategory.AbilityDamage => "Abilities",
            FlyTextCategory.AbilityHealing => "Heals",
            FlyTextCategory.Miss => "Misses",
            FlyTextCategory.Buff => "Buffs",
            FlyTextCategory.Debuff => "Debuffs",
            FlyTextCategory.CC => "Crowd Control",
            _ => throw new ArgumentOutOfRangeException(nameof(category), category, null),
        };
    }
}