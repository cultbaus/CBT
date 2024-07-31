namespace CBT.FlyText.Types;

using System;
using System.Collections.Generic;
using System.Linq;

[Flags]
internal enum FlyTextCategory
{
    // Groups
    Combat = 1 << 0,
    NonCombat = 1 << 1,

    // Categories
    AutoAttack = 1 << 10 | Combat,
    AbilityDamage = 1 << 11 | Combat,
    AbilityHealing = 1 << 12 | Combat,
    Miss = 1 << 13 | Combat,
}

internal static class FlyTextCategoryMethods
{
    internal static List<FlyTextKind> GetKindsForCategory(this FlyTextCategory category)
        => IsCategory(category)
            ? Enum.GetValues<FlyTextKind>()
                .Cast<FlyTextKind>()
                .Where(value => value.GetCategory() == category)
                .ToList()
            : throw new ArgumentOutOfRangeException(nameof(category), category, null);

    internal static List<FlyTextKind> GetKindsForGroup(this FlyTextCategory group)
        => IsGroup(group)
            ? Enum.GetValues<FlyTextKind>()
                .Cast<FlyTextKind>()
                .Where(value => value.GetCategory().HasFlag(group))
                .ToList()
            : throw new ArgumentOutOfRangeException(nameof(group), group, null);

    internal static List<FlyTextCategory> GetCategoriesForGroup(this FlyTextCategory group)
        => IsGroup(group)
        ? Enum.GetValues<FlyTextCategory>()
            .Cast<FlyTextCategory>()
            .Where(value => value.HasFlag(group))
            .ToList()
        : throw new ArgumentOutOfRangeException(nameof(group), group, null);

    internal static bool IsGroup(this FlyTextCategory value)
        => FlyTextCategory.Combat == value || FlyTextCategory.NonCombat == value;

    internal static bool IsCategory(this FlyTextCategory value)
        => !IsGroup(value);
}