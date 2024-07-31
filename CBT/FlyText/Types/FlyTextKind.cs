namespace CBT.FlyText.Types;

using System;

using S = Dalamud.Game.Gui.FlyText;

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

internal enum FlyTextKind
{
    [FlyTextCategory(FlyTextCategory.AutoAttack)]
    AutoAttackOrDot = S.FlyTextKind.AutoAttackOrDot,

    [FlyTextCategory(FlyTextCategory.AutoAttack)]
    AutoAttackOrDotDh = S.FlyTextKind.AutoAttackOrDotDh,

    [FlyTextCategory(FlyTextCategory.AutoAttack)]
    AutoAttackOrDotCrit = S.FlyTextKind.AutoAttackOrDotCrit,

    [FlyTextCategory(FlyTextCategory.AutoAttack)]
    AutoAttackOrDotCritDh = S.FlyTextKind.AutoAttackOrDotCritDh,

    [FlyTextCategory(FlyTextCategory.AbilityDamage)]
    Damage = S.FlyTextKind.Damage,

    [FlyTextCategory(FlyTextCategory.AbilityDamage)]
    DamageDh = S.FlyTextKind.DamageDh,

    [FlyTextCategory(FlyTextCategory.AbilityDamage)]
    DamageCrit = S.FlyTextKind.DamageCrit,

    [FlyTextCategory(FlyTextCategory.AbilityDamage)]
    DamageCritDh = S.FlyTextKind.DamageCritDh,

    [FlyTextCategory(FlyTextCategory.Miss)]
    Miss = S.FlyTextKind.Miss,

    [FlyTextCategory(FlyTextCategory.Miss)]
    NamedMiss = S.FlyTextKind.NamedMiss,

    [FlyTextCategory(FlyTextCategory.Miss)]
    Dodge = S.FlyTextKind.Dodge,

    [FlyTextCategory(FlyTextCategory.Miss)]
    NamedDodge = S.FlyTextKind.NamedDodge,

    [FlyTextCategory(FlyTextCategory.Buff)]
    Buff = S.FlyTextKind.Buff,

    [FlyTextCategory(FlyTextCategory.Debuff)]
    Debuff = S.FlyTextKind.Debuff,

    // Exp,
    // IslandExp,
    // MpDrain,
    // NamedTp,

    [FlyTextCategory(FlyTextCategory.AbilityHealing)]
    Healing = S.FlyTextKind.Healing,

    // MpRegen,
    // NamedTp2,
    // EpRegen,
    // CpRegen,
    // GpRegen,
    // None,
    // Invulnerable,
    // Interrupted,
    // CraftingProgress,
    // CraftingQuality,
    // CraftingQualityCrit,

    [FlyTextCategory(FlyTextCategory.AbilityHealing)]
    HealingCrit = S.FlyTextKind.HealingCrit,

    [FlyTextCategory(FlyTextCategory.Miss)]
    DebuffNoEffect = S.FlyTextKind.DebuffNoEffect,

    [FlyTextCategory(FlyTextCategory.Buff)]
    BuffFading = S.FlyTextKind.BuffFading,

    [FlyTextCategory(FlyTextCategory.Debuff)]
    DebuffFading = S.FlyTextKind.DebuffFading,

    // Named,

    [FlyTextCategory(FlyTextCategory.Miss)]
    DebuffResisted = S.FlyTextKind.DebuffResisted,

    [FlyTextCategory(FlyTextCategory.CC)]
    Incapacitated = S.FlyTextKind.Incapacitated,

    [FlyTextCategory(FlyTextCategory.Miss)]
    FullyResisted = S.FlyTextKind.FullyResisted,

    [FlyTextCategory(FlyTextCategory.Miss)]
    HasNoEffect = S.FlyTextKind.HasNoEffect,

    [FlyTextCategory(FlyTextCategory.AbilityHealing)]
    HpDrain = S.FlyTextKind.HpDrain,

    [FlyTextCategory(FlyTextCategory.Miss)]
    DebuffInvulnerable = S.FlyTextKind.DebuffInvulnerable,

    [FlyTextCategory(FlyTextCategory.Miss)]
    Resist = S.FlyTextKind.Resist,

    // LootedItem,
    // Collectability,
    // CollectabilityCrit,
    // Reflect,
    // Reflected,
    // CraftingQualityDh,
    // CraftingQualityCritDh
}