namespace CBT.Types;

using System;
using System.Collections.Generic;
using System.Linq;

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
    /// None group.
    /// </summary>
    GroupNone = 1 << 2,

    // Categories

    /// <summary>
    /// AutoAttack category. Member of the Combat group.
    /// </summary>
    AutoAttack = 1 << 10 | Combat,

    /// <summary>
    /// AbilityDamage category. Member of the Combat group.
    /// </summary>
    AbilityDamage = 1 << 11 | Combat,

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

    /// <summary>
    /// CC category. Member of the Combat group.
    /// </summary>
    CategoryNone = 1 << 17 | GroupNone,
}

/// <summary>
/// FlyTextCategory methods.
/// </summary>
public static class FlyTextCategoryExtension
{
    /// <summary>
    /// Determines if a FlyTextCategory is a Group sub-type.
    /// </summary>
    /// <param name="value">FlyTextCategory to check.</param>
    /// <returns>True if the category is a group sub-type.</returns>
    public static bool IsGroup(this FlyTextCategory value)
        => value == FlyTextCategory.Combat || value == FlyTextCategory.NonCombat;

    /// <summary>
    /// Determines if a FlyTextCategory is a Category sub-type.
    /// </summary>
    /// <param name="value">FlyTextCategory to check.</param>
    /// <returns>True if the category is a category sub-type.</returns>
    public static bool IsCategory(this FlyTextCategory value)
        => !IsGroup(value);

    /// <summary>
    /// Get kinds for a category or group.
    /// </summary>
    /// <param name="category">Category or group.</param>
    /// <returns>Enumerable result of kinds in that category or group.</returns>
    public static IEnumerable<FlyTextKind> GetKindsFor(FlyTextCategory category)
        => Enum.GetValues<FlyTextKind>()
            .Cast<FlyTextKind>()
            .Where(kind => IsCategory(category) ? kind.InCategory(category) : kind.InGroup(category));

    /// <summary>
    /// Get categories for a group.
    /// </summary>
    /// <param name="group">The group to iterate.</param>
    /// <returns>Enumerable result of categories in that group.</returns>
    public static IEnumerable<FlyTextCategory> GetCategoriesFor(FlyTextCategory group)
        => Enum.GetValues<FlyTextCategory>()
            .Cast<FlyTextCategory>()
            .Where(category => category.IsCategory() && category.HasFlag(group));

    /// <summary>
    /// Implements a filter over fly text categories.
    /// </summary>
    /// <param name="category">The category to filter over.</param>
    /// <param name="predicate">The predicate to filter by.</param>
    /// <returns>The collection of fly text kinds which have been filtered.</returns>
    public static IEnumerable<FlyTextKind> Where(this FlyTextCategory category, Predicate<FlyTextKind> predicate)
        => GetKindsFor(category)
            .Where(kind => predicate(kind));

    /// <summary>
    /// Iterates the kinds of a category or group and performs an action.
    /// </summary>
    /// <param name="category">The category or group to iterate.</param>
    /// <param name="action">The action to perform.</param>
    public static void ForEachKind(this FlyTextCategory category, Action<FlyTextKind> action)
    {
        GetKindsFor(category)
           .ToList()
           .ForEach(action);
    }

    /// <summary>
    /// Iterates the kinds of a category or group and performs an action.
    /// </summary>
    /// <param name="category">The category or group to iterate.</param>
    /// <param name="action">The action to perform.</param>
    public static void ForEachCategory(this FlyTextCategory category, Action<FlyTextCategory> action)
    {
        GetCategoriesFor(category)
           .ToList()
           .ForEach(action);
    }

    /// <summary>
    /// Gets all categories.
    /// </summary>
    /// <returns>All categories.</returns>
    public static IEnumerable<FlyTextCategory> GetAllCategories()
        => Enum.GetValues<FlyTextCategory>()
            .Cast<FlyTextCategory>()
            .Where(category => category.IsCategory());

    /// <summary>
    /// Gets all groups.
    /// </summary>
    /// <returns>All groups.</returns>
    public static IEnumerable<FlyTextCategory> GetAllGroups()
        => Enum.GetValues<FlyTextCategory>()
            .Cast<FlyTextCategory>()
            .Where(group => group.IsGroup());
}