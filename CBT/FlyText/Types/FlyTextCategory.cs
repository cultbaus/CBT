namespace CBT.FlyText.Types;

using System;
using System.Collections.Generic;
using System.Linq;

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

    internal static void ForEach(this FlyTextCategory category, Action<FlyTextKind> action)
    {
        Enum.GetValues<FlyTextKind>()
           .Cast<FlyTextKind>()
           .Where(kind => IsCategory(category) ? kind.InCategory(category) : kind.InGroup(category))
           .ToList()
           .ForEach(action);
    }
}

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
internal class FlyTextCategoryAttribute(FlyTextCategory category) : Attribute
{
    public FlyTextCategory Category { get; } = category;
}

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
    Buff = 1 << 14 | Combat,
    Debuff = 1 << 15 | Combat,
    CC = 1 << 16 | Combat,
}