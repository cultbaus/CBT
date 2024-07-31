namespace Scroll.FlyText.Types;

using System;
using S = Dalamud.Game.Gui.FlyText;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
internal class FlyTextCategoryAttribute(FlyTextCategory category) : Attribute
{
    public FlyTextCategory Category { get; } = category;
}

internal enum FlyTextKind
{
    [FlyTextCategory(FlyTextCategory.Attack)]
    AutoAttackOrDot = S.FlyTextKind.AutoAttackOrDot,

    [FlyTextCategory(FlyTextCategory.Attack)]
    AutoAttackOrDotDh = S.FlyTextKind.AutoAttackOrDotDh,

    [FlyTextCategory(FlyTextCategory.Attack)]
    AutoAttackOrDotCrit = S.FlyTextKind.AutoAttackOrDotCrit,

    [FlyTextCategory(FlyTextCategory.Attack)]
    AutoAttackOrDotCritDh = S.FlyTextKind.AutoAttackOrDotCritDh,

    [FlyTextCategory(FlyTextCategory.AbilityDamage)]
    Damage = S.FlyTextKind.Damage,

    [FlyTextCategory(FlyTextCategory.AbilityDamage)]
    DamageDh = S.FlyTextKind.DamageDh,

    [FlyTextCategory(FlyTextCategory.AbilityDamage)]
    DamageCrit = S.FlyTextKind.DamageCrit,

    [FlyTextCategory(FlyTextCategory.AbilityDamage)]
    DamageCritDh = S.FlyTextKind.DamageCritDh,
}

internal static class FlyTextKindMethods
{
    internal static FlyTextCategory GetCategory(this FlyTextKind kind)
    {
        var attr = typeof(FlyTextKind)
            .GetMember(kind.ToString())[0]
            .GetCustomAttributes(typeof(FlyTextCategoryAttribute), false);

        return attr.Length > 0
            ? ((FlyTextCategoryAttribute)attr[0]).Category
            : throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
    }

    internal static bool InCategory(this FlyTextKind kind, FlyTextCategory category)
        => category.IsCategory()
            ? GetCategory(kind) == category
            : throw new ArgumentOutOfRangeException(nameof(category), category, null);

    internal static bool InGroup(this FlyTextKind kind, FlyTextCategory group)
        => group.IsGroup()
            ? GetCategory(kind).HasFlag(group)
            : throw new ArgumentOutOfRangeException(nameof(group), group, null);
}
