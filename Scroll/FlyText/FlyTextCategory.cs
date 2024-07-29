namespace Scroll.FlyText;

using System;

using S = Dalamud.Game.Gui.FlyText;

[Flags]
internal enum FlyTextCategory
{
    Combat = 1 << 0,
    NonCombat = 1 << 1,

    Attack = 1 << 10 | Combat,
    AbilityDamage = 1 << 11 | Combat,
    AbilityHealing = 1 << 12 | Combat,
    Miss = 1 << 13 | Combat,
}

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