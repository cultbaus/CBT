namespace CBT.FlyText.Types;

using System;

using S = Dalamud.Game.Gui.FlyText;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
internal class FlyTextCategoryAttribute(FlyTextCategory category) : Attribute
{
    public FlyTextCategory Category { get; } = category;
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

    // Miss,
    // NamedMiss,
    // Dodge,
    // NamedDodge,
    // Buff,
    // Debuff,
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
    // DebuffNoEffect,
    // BuffFading,
    // DebuffFading,
    // Named,
    // DebuffResisted,
    // Incapacitated,
    // FullyResisted,
    // HasNoEffect,
    // HpDrain,
    // DebuffInvulnerable,
    // Resist,
    // LootedItem,
    // Collectability,
    // CollectabilityCrit,
    // Reflect,
    // Reflected,
    // CraftingQualityDh,
    // CraftingQualityCritDh
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
